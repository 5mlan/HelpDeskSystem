# IT Help Desk System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4)](https://learn.microsoft.com/aspnet/core/)
[![SQLite](https://img.shields.io/badge/Database-SQLite-003B57?logo=sqlite)](https://www.sqlite.org/)

A web-based IT support ticket management system built with **ASP.NET Core MVC**, **Entity Framework Core**, **Identity**, and **SQLite**.

The system allows users to report technical issues, technicians to manage and resolve tickets, and administrators to monitor the system and manage users.

## Features

- Secure registration and login.
- User, Technician, and Administrator roles.
- Create and manage support tickets.
- Assign tickets to technicians.
- Ticket priority and status tracking.
- Automatic SLA deadlines based on priority.
- Overdue ticket indicators.
- Comments inside tickets.
- Complete ticket activity history.
- Search and filter tickets.
- Dashboard with ticket statistics.
- User and role management.
- Export tickets to CSV.
- Responsive Arabic interface.
- Automatic SQLite database creation.
- Docker support.
- GitHub Actions build checks.

## User Roles

| Role | Permissions |
|------|-------------|
| User | Create tickets, view personal tickets, add comments, and close resolved tickets |
| Technician | View tickets, assign tickets, update status and priority, and export reports |
| Administrator | Full ticket access, dashboard, user management, roles, and reports |

## Technologies

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity
- SQLite
- Razor Views
- Bootstrap
- HTML, CSS, and JavaScript
- Docker
- GitHub Actions

## Project Structure

```text
HelpDeskSystem/
├── Controllers/       # Application actions and request handling
├── Data/              # Database context and seed data
├── Models/            # Database entities
├── ViewModels/        # Interface-specific models
├── Views/             # Razor pages
├── Services/          # Business logic
├── wwwroot/           # CSS, JavaScript, and static files
├── docs/              # Project documentation
├── .github/           # GitHub workflows and templates
├── Program.cs         # Application startup and configuration
├── appsettings.json   # Application and database settings
└── Dockerfile         # Docker configuration
```

## How the System Works

1. The application starts from `Program.cs`.
2. ASP.NET Core Identity handles authentication and user roles.
3. Entity Framework Core connects the application to SQLite.
4. The database and default administrator account are created automatically.
5. A user can register and create a support ticket.
6. The system calculates the SLA deadline according to ticket priority.
7. A technician assigns the ticket and updates its status.
8. Every important change is saved in the ticket activity history.
9. Administrators can manage users, view statistics, and export reports.

## Requirements

- Visual Studio 2022.
- ASP.NET and web development workload.
- .NET 8 SDK.
- Git, if you want to clone the repository.

A separate database server is not required because the project uses SQLite.

## Installation

Clone the repository:

```bash
git clone https://github.com/5mlan/it-help-desk-system.git
cd it-help-desk-system
```

### Run with Visual Studio

1. Open `HelpDeskSystem.sln` in Visual Studio 2022.
2. Wait for NuGet packages to restore.
3. Select `HelpDeskSystem` as the startup project.
4. Press `F5` or click **Run**.
5. The SQLite database will be created automatically.

### Run from the Command Line

```bash
dotnet restore
dotnet run --project HelpDeskSystem/HelpDeskSystem.csproj
```

## Demo Administrator Account

```text
Email: admin@helpdesk.local
Password: Admin123!
```

> Change or remove the demo administrator credentials before publishing the application online.

## Ticket Workflow

```text
Open → Assigned → In Progress → Resolved → Closed
```

A ticket can contain:

- Title and description.
- Category.
- Priority.
- Current status.
- Ticket creator.
- Assigned technician.
- SLA deadline.
- Comments.
- Activity history.

## Database

The main database entities are:

- **Users:** User accounts and roles.
- **Tickets:** Technical support requests.
- **Ticket Comments:** Communication between users and technicians.
- **Ticket Activities:** History of changes made to each ticket.

Entity Framework Core manages all database relationships and stores the information in SQLite.

## CSV Export

Technicians and administrators can export ticket data to a UTF-8 CSV file.

The exported file supports Arabic text and can be opened using Microsoft Excel or other spreadsheet applications.

## Docker

Build the Docker image:

```bash
docker build -t help-desk-system .
```

Run the application:

```bash
docker run --name help-desk-system -p 8080:8080 help-desk-system
```

Open:

```text
http://localhost:8080
```

## Future Improvements

- Email notifications.
- File and screenshot attachments.
- Password reset by email.
- PDF reports.
- Advanced dashboard analytics.
- REST API.
- Mobile application.
- Cloud deployment.
- Unit and integration tests.

## Security Notes

- Do not upload passwords or private keys to GitHub.
- Store production secrets in environment variables.
- Change the default administrator password.
- Use HTTPS when deploying the application.
- Update NuGet packages regularly.

## Contributing

Contributions and suggestions are welcome.

You can open an issue or submit a pull request with a clear description of the proposed change.

## Author

**Salem Al-Mokhles**  
Computer Information Systems Graduate

- GitHub: [@5mlan](https://github.com/5mlan)
- LinkedIn: [Salem Al-Mokhles](https://www.linkedin.com/in/%D8%B3%D8%A7%D9%84%D9%85-%D8%A7%D9%84-%D9%85%D8%AE%D9%84%D8%B5-768329395)

## نبذة بالعربية

نظام إلكتروني متكامل لإدارة بلاغات الدعم الفني. يتيح للمستخدم إنشاء التذاكر ومتابعتها، وللفني استلامها وتحديث حالتها والتواصل مع صاحب البلاغ، بينما يستطيع المدير متابعة الإحصائيات وإدارة المستخدمين والصلاحيات وتصدير التقارير.

تم تطوير المشروع باستخدام **ASP.NET Core MVC وSQLite**، ويمكن تشغيله مباشرة باستخدام Visual Studio 2022 دون الحاجة إلى تثبيت خادم قاعدة بيانات منفصل.

---

If you find this project useful, consider giving it a ⭐.
