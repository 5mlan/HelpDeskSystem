namespace HelpDesk.Web.Models;

public static class TicketSla
{
    public static DateTime CalculateDueAt(DateTime createdAt, TicketPriority priority) => priority switch
    {
        TicketPriority.Critical => createdAt.AddHours(4),
        TicketPriority.High => createdAt.AddHours(8),
        TicketPriority.Medium => createdAt.AddHours(24),
        TicketPriority.Low => createdAt.AddHours(72),
        _ => createdAt.AddHours(24)
    };

    public static bool IsOverdue(Ticket ticket) =>
        ticket.Status is not TicketStatus.Resolved and not TicketStatus.Closed
        && ticket.DueAt < DateTime.UtcNow;
}
