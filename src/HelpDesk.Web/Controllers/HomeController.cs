using HelpDesk.Web.Data;
using HelpDesk.Web.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        var query = _db.Tickets.AsNoTracking();

        if (!User.IsInRole("Admin") && !User.IsInRole("Technician"))
            query = query.Where(x => x.UserId == userId);

        var model = new DashboardViewModel
        {
            Total = await query.CountAsync(),
            Open = await query.CountAsync(x => x.Status == TicketStatus.Open),
            InProgress = await query.CountAsync(x => x.Status == TicketStatus.InProgress),
            Resolved = await query.CountAsync(x => x.Status == TicketStatus.Resolved || x.Status == TicketStatus.Closed),
            Critical = await query.CountAsync(x => x.Priority == TicketPriority.Critical && x.Status != TicketStatus.Closed),
            Overdue = await query.CountAsync(x => x.Status != TicketStatus.Resolved && x.Status != TicketStatus.Closed && x.DueAt < DateTime.UtcNow),
            RecentTickets = await query
                .Include(x => x.User)
                .Include(x => x.AssignedTo)
                .OrderByDescending(x => x.UpdatedAt)
                .Take(6)
                .ToListAsync()
        };

        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Error() => View();
}
