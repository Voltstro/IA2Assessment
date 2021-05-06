# IA2Assessment Project Files

This is my project files for the programming part of my [Digital Solutions IA2 Assessment](https://www.qcaa.qld.edu.au/senior/senior-subjects/technologies/digital-solutions/assessment).

I codded this part using [ASP.NET Core 5](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0) instead of PHP due to the fact that I have a better understanding of C#, and that I wanted to learn ASP.NET Core.

This project is a *little* bit more advance than what the assignment asked for, the assignment asked for just to make it look like its doing stuff and that it is all faked.
However I wanted to learn ASP.NET so it includes user identity system, entity framework, as well as a functional ordering system.

I have made the programming part of my project public incase anyone wants a basic understanding on how this might be done. Your assessment task maybe slightly different to making a tuckshop ordering system, but this still has what you need to understand.

## Setup

1. Have a MySQL server ready, with a database called 'tuckshop'.

2. Update `appsettings.json` to include the connection string to the MySQL server.

3. Install the [EF tool](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

4. Run `dotnet ef database update`

5. Open the sln with Rider or VS. Run the project WITHOUT IIS express, it will automatically add an account to the database called `admin` with the password of `Password.1234`.

6. Update your database with the menu items you want.

There, that should have given you a basic idea on how to setup this project.

# License

This project is under the MIT license. See the [LICENSE.md](/LICENSE.md) file for more information.

# Disclamier

If you just copy this project to slap into your assessment cause you got a similar assignment task, then your a cunt, and per the license, I take no responsibility for you getting called out on it, if you do. If you are a teacher that wants to use this as a demonstration or some shit, then go ahead.