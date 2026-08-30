using HelpDesk.Web.Data;
using HelpDesk.Web.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HelpDesk.Web.Controllers;

[Authorize]
public class TicketsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? search, TicketStatus? status, TicketPriority? priority)
    {
        var userId = _userManager.GetUserId(User)!;
        var query = _db.Tickets
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.AssignedTo)
            .AsQueryable();

        if (!IsSupportStaff())
            query = query.Where(x => x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Title.Contains(search) || x.Description.Contains(search));

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(x => x.Priority == priority.Value);

        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.Priority = priority;

        return View(await query.OrderByDescending(x => x.UpdatedAt).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _db.Tickets
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.AssignedTo)
            .Include(x => x.Comments.OrderBy(c => c.CreatedAt))
                .ThenInclude(c => c.User)
            .Include(x => x.Activities.OrderByDescending(a => a.CreatedAt))
                .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ticket is null)
            return NotFound();

        if (!CanAccess(ticket))
            return Forbid();

        return View(ticket);
    }

    public IActionResult Create() => View(new TicketFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var ticket = new Ticket
        {
            Title = model.Title.Trim(),
            Description = model.Description.Trim(),
            Category = model.Category,
            Priority = model.Priority,
            UserId = _userManager.GetUserId(User)!,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        ticket.DueAt = TicketSla.CalculateDueAt(ticket.CreatedAt, ticket.Priority);

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
        AddActivity(ticket.Id, TicketActivityType.Created, "أنشأ التذكرة.");
        await _db.SaveChangesAsync();
        TempData["Success"] = $"تم إنشاء التذكرة رقم #{ticket.Id} بنجاح.";
        return RedirectToAction(nameof(Details), new { id = ticket.Id });
    }

    [HttpPost, Authorize(Roles = "Admin,Technician"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, TicketStatus status)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        if (ticket.Status != status)
        {
            var previousStatus = TicketDisplay.StatusText(ticket.Status);
            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;
            AddActivity(ticket.Id, TicketActivityType.StatusChanged,
                $"غيّر الحالة من {previousStatus} إلى {TicketDisplay.StatusText(status)}.");
            await _db.SaveChangesAsync();
        }
        TempData["Success"] = "تم تحديث حالة التذكرة.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, Authorize(Roles = "Admin,Technician"), ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignToMe(int id)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User)!;
        var wasAssignedToCurrentUser = ticket.AssignedToId == currentUserId;
        ticket.AssignedToId = currentUserId;
        if (ticket.Status == TicketStatus.Open)
            ticket.Status = TicketStatus.InProgress;
        ticket.UpdatedAt = DateTime.UtcNow;
        if (!wasAssignedToCurrentUser)
            AddActivity(ticket.Id, TicketActivityType.Assigned, "استلم التذكرة وبدأ معالجتها.");
        await _db.SaveChangesAsync();
        TempData["Success"] = "تم إسناد التذكرة إليك.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int id, string comment)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        if (!CanAccess(ticket))
            return Forbid();

        comment = comment?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(comment) || comment.Length > 2000)
        {
            TempData["Error"] = "اكتب تعليقًا من 1 إلى 2000 حرف.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _db.TicketComments.Add(new TicketComment
        {
            TicketId = id,
            UserId = _userManager.GetUserId(User)!,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        });
        AddActivity(ticket.Id, TicketActivityType.Commented, "أضاف تعليقًا جديدًا.");
        ticket.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (!IsSupportStaff() && ticket.UserId != userId)
            return Forbid();

        if (ticket.Status != TicketStatus.Closed)
        {
            ticket.Status = TicketStatus.Closed;
            ticket.UpdatedAt = DateTime.UtcNow;
            AddActivity(ticket.Id, TicketActivityType.Closed, "أغلق التذكرة.");
            await _db.SaveChangesAsync();
        }
        TempData["Success"] = "تم إغلاق التذكرة.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Roles = "Admin,Technician")]
    public async Task<IActionResult> ExportCsv(string? search, TicketStatus? status, TicketPriority? priority)
    {
        var query = _db.Tickets
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.AssignedTo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Title.Contains(search) || x.Description.Contains(search));
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);
        if (priority.HasValue)
            query = query.Where(x => x.Priority == priority.Value);

        var tickets = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        var csv = new StringBuilder();
        csv.AppendLine("الرقم,العنوان,مقدم الطلب,التصنيف,الأولوية,الحالة,المسند إليه,تاريخ الإنشاء,المهلة");

        foreach (var ticket in tickets)
        {
            csv.AppendLine(string.Join(",", new[]
            {
                ticket.Id.ToString(),
                Csv(ticket.Title),
                Csv(ticket.User?.FullName ?? string.Empty),
                Csv(TicketDisplay.CategoryText(ticket.Category)),
                Csv(TicketDisplay.PriorityText(ticket.Priority)),
                Csv(TicketDisplay.StatusText(ticket.Status)),
                Csv(ticket.AssignedTo?.FullName ?? "غير مسندة"),
                Csv(TicketDisplay.LocalDate(ticket.CreatedAt)),
                Csv(TicketDisplay.LocalDate(ticket.DueAt))
            }));
        }

        var bytes = Encoding.UTF8.GetBytes("\uFEFF" + csv);
        return File(bytes, "text/csv; charset=utf-8", $"helpdesk-tickets-{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    private bool IsSupportStaff() => User.IsInRole("Admin") || User.IsInRole("Technician");

    private bool CanAccess(Ticket ticket) =>
        IsSupportStaff() || ticket.UserId == _userManager.GetUserId(User);

    private void AddActivity(int ticketId, TicketActivityType type, string description)
    {
        _db.TicketActivities.Add(new TicketActivity
        {
            TicketId = ticketId,
            UserId = _userManager.GetUserId(User)!,
            Type = type,
            Description = description,
            CreatedAt = DateTime.UtcNow
        });
    }

    private static string Csv(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
}
