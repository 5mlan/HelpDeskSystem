using HelpDesk.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.ViewModels;

public class TicketFormViewModel
{
    [Required(ErrorMessage = "عنوان المشكلة مطلوب")]
    [StringLength(200)]
    [Display(Name = "عنوان المشكلة")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "وصف المشكلة مطلوب")]
    [StringLength(4000)]
    [Display(Name = "وصف المشكلة")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "التصنيف")]
    public TicketCategory Category { get; set; } = TicketCategory.Other;

    [Display(Name = "الأولوية")]
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}

public class DashboardViewModel
{
    public int Total { get; set; }
    public int Open { get; set; }
    public int InProgress { get; set; }
    public int Resolved { get; set; }
    public int Critical { get; set; }
    public int Overdue { get; set; }
    public List<Ticket> RecentTickets { get; set; } = new();
}

public class UserRoleViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
