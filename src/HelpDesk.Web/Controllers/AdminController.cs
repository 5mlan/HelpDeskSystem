using HelpDesk.Web.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.AsNoTracking().OrderBy(x => x.FullName).ToListAsync();
        var model = new List<UserRoleViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserRoleViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User",
                CreatedAt = user.CreatedAt
            });
        }

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(string id, string role)
    {
        var allowedRoles = new[] { "Admin", "Technician", "User" };
        if (!allowedRoles.Contains(role))
            return BadRequest();

        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == user.Id && role != "Admin")
        {
            TempData["Error"] = "لا يمكنك إزالة صلاحية المدير من حسابك الحالي.";
            return RedirectToAction(nameof(Users));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        TempData["Success"] = "تم تحديث صلاحية المستخدم.";
        return RedirectToAction(nameof(Users));
    }
}
