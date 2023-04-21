# Development Environment Setup

- [HOME](ReadMe.md)

## Prerequsites:
- Access to BitBucket Projects: (TenantManager, Menominee)
- Telerik license setup
- `localhost` SQL Server Developer instance
## Databases
Clone the TenantManager and Menominee projects locally
### Menominee API Database
This is found in the CustomerVehicleManagement.Api project as entity framework migrations

1. Start with the Menominee solution
2. Check the Menominee project settings:

    1. \Data\ApplicationDbContext.cs comment out lines of OnConfiguring regaring unit tests
    2. \StartUp.cs un/comment lines at the end of `ConfigureServices` regarding the `ApplicationDbContext`

3. Add the [Telerik Nuget Source](https://docs.telerik.com/blazor-ui/installation/nuget)
4. Optionally run `dotnet restore` for the API Project
5. Run a database update with Entity Framework migrations from the solution root with the command line: `dotnet ef database update -p CustomerVehicleManagement.Api/CustomerVehicleManagement.Api.csproj
   --context CustomerVehicleManagement.Api.Data.ApplicationDbContext`
6. Check your server for the new database named Menominee

### Identity Database
This is found in the Janco.Idp project as entity framework migrations

1. Start with the Menominee solution
2. Open a command line at the solution root and run the update script: `dotnet ef database update -p Janco.Idp/Janco.Idp.csproj`
3. Check your server for the new database named JancoIdentity and look for the AspNet identity tables
   
### Tenant Database
This is in a separate solution

1. Start with the TenantManager (it's a console app) Solution
2. Check this key in AppSettings: `<add key="IsDevelopment" value="true" />`
3. Run the console app > answer all the questions to create a new Database and user
4. Check your server for the new database named according to your choices in the script

## Web Application
setup multiple startup projects:

1. CustomerVehicleManagement.Api
2. Janco.Idp
3. Menominee.Client

Start up and you should be able to login with the login set up during the Tenant Database Setup 