# Contributing

Thank you for improving the IT Help Desk System.

## Local setup

1. Fork and clone the repository.
2. Install the .NET 8 SDK.
3. Open `HelpDeskSystem.sln` in Visual Studio 2022.
4. Restore NuGet packages and run the `HelpDesk.Web` project.

## Branches and commits

- Create a branch such as `feature/ticket-attachments` or `fix/login-validation`.
- Keep each commit focused on one logical change.
- Use clear commit messages, for example: `feat: add ticket export`.

## Before opening a pull request

Run:

```bash
dotnet restore HelpDeskSystem.sln
dotnet build HelpDeskSystem.sln --configuration Release --no-restore
```

Do not commit local databases, passwords, `.env` files, `bin`, `obj`, or `.vs` folders.
