using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.Models;

public class TicketActivity
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public TicketActivityType Type { get; set; }

    [Required, StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
