-- The application creates the SQLite database automatically through Entity Framework Core.
-- This file documents the business tables; ASP.NET Identity creates the user/role tables.

CREATE TABLE Tickets (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    Category TEXT NOT NULL,
    Priority TEXT NOT NULL,
    Status TEXT NOT NULL,
    UserId TEXT NOT NULL,
    AssignedToId TEXT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL,
    DueAt TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (AssignedToId) REFERENCES AspNetUsers(Id) ON DELETE SET NULL
);

CREATE TABLE TicketComments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TicketId INTEGER NOT NULL,
    UserId TEXT NOT NULL,
    Comment TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (TicketId) REFERENCES Tickets(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE RESTRICT
);

CREATE TABLE TicketActivities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TicketId INTEGER NOT NULL,
    UserId TEXT NOT NULL,
    Type TEXT NOT NULL,
    Description TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (TicketId) REFERENCES Tickets(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE RESTRICT
);
