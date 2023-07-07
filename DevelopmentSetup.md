# Development Environment Setup

- [HOME](ReadMe.md)

## Prerequsites:
- Access to BitBucket Projects: (TenantManager, Menominee)
- Telerik license setup
- `localhost` SQL Server Developer instance
- Developer Admin account in Azure AD B2C
## Databases
Clone the TenantManager and Menominee projects locally
### Menominee API Database
This is found in the CustomerVehicleManagement.Api project as entity framework migrations

1. Start with the Menominee solution

2. Add the [Telerik Nuget Source](https://docs.telerik.com/blazor-ui/installation/nuget)
3. Optionally run `dotnet restore` for the API Project
4. Run a database update with Entity Framework migrations from the solution root with the command line: `dotnet ef database update -p CustomerVehicleManagement.Api/CustomerVehicleManagement.Api.csproj
   --context CustomerVehicleManagement.Api.Data.ApplicationDbContext`
5. Check your server for the new database named Menominee-stage

### Tenant Database
This is in a separate solution

1. Start with the TenantManager (it's a console app) Solution
2. Check this key in AppSettings: `<add key="IsDevelopment" value="true" />`
3. Run the console app > answer all the questions to create a new Database and user
4. Check your server for the new database named according to your choices in the script

## Web Application
startup with the API project (CustomerVehicleManagement.Api) which serves the Client as well 

Start up and you should be able to login with Azure AD B2C login 