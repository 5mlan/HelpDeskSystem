using HelpDesk.Web.Models;

namespace HelpDesk.Web.ViewModels;

public static class TicketDisplay
{
    public static string StatusText(TicketStatus value) => value switch
    {
        TicketStatus.Open => "مفتوحة",
        TicketStatus.InProgress => "قيد المعالجة",
        TicketStatus.Resolved => "تم الحل",
        TicketStatus.Closed => "مغلقة",
        _ => value.ToString()
    };

    public static string PriorityText(TicketPriority value) => value switch
    {
        TicketPriority.Low => "منخفضة",
        TicketPriority.Medium => "متوسطة",
        TicketPriority.High => "عالية",
        TicketPriority.Critical => "حرجة",
        _ => value.ToString()
    };

    public static string CategoryText(TicketCategory value) => value switch
    {
        TicketCategory.Network => "الشبكة",
        TicketCategory.Hardware => "الأجهزة",
        TicketCategory.Software => "البرامج",
        TicketCategory.Account => "الحسابات",
        TicketCategory.Other => "أخرى",
        _ => value.ToString()
    };

    public static string StatusClass(TicketStatus value) => $"status-{value.ToString().ToLowerInvariant()}";
    public static string PriorityClass(TicketPriority value) => $"priority-{value.ToString().ToLowerInvariant()}";
    public static string LocalDate(DateTime value) => value.ToLocalTime().ToString("yyyy/MM/dd - hh:mm tt");

    public static string ActivityIcon(TicketActivityType value) => value switch
    {
        TicketActivityType.Created => "+",
        TicketActivityType.StatusChanged => "↻",
        TicketActivityType.Assigned => "→",
        TicketActivityType.Commented => "✎",
        TicketActivityType.Closed => "✓",
        _ => "•"
    };
}
