namespace HelpDesk.Web.Models;

public enum TicketCategory
{
    Network,
    Hardware,
    Software,
    Account,
    Other
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}

public enum TicketActivityType
{
    Created,
    StatusChanged,
    Assigned,
    Commented,
    Closed
}
