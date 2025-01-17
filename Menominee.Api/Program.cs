using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Menominee.Api;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Businesses;
using Menominee.Api.Features.Contactables.Persons;
using Menominee.Api.Features.CreditCards;
using Menominee.Api.Features.Customers;
using Menominee.Api.Features.Employees;
using Menominee.Api.Features.Inventory;
using Menominee.Api.Features.Manufacturers;
using Menominee.Api.Features.Payables.Invoices;
using Menominee.Api.Features.Payables.PaymentMethods;
using Menominee.Api.Features.Payables.Vendors;
using Menominee.Api.Features.ProductCodes;
using Menominee.Api.Features.RepairOrders;
using Menominee.Api.Features.SaleCodes;
using Menominee.Api.Features.SellingPriceNames;
using Menominee.Api.Features.Settings;
using Menominee.Api.Features.Taxes;
using Menominee.Api.Features.Users;
using Menominee.Api.Features.Vehicles;
using Menominee.Shared;
using Menominee.Shared.Models.Tenants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;

    var storageConnection = builder.Configuration["app-log-storage-connection"];
    var storageContainerName = builder.Environment.EnvironmentName switch
    {
        "Staging" => "api-logs-staging",
        "Development" => "api-logs-dev",
        _ => "api-logs"
    };

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.AzureBlobStorage(
        connectionString: storageConnection,
        storageContainerName: storageContainerName,
        storageFileName: "menominee-api-log-{yyyy}-{MM}-{dd}.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

    IdentityModelEventSource.ShowPII = builder.Environment.IsDevelopment();

    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        // Our (Microsoft) recommendation is to use a vault per application per environment (Development, Pre-Production, and Production).
        builder.Configuration.AddAzureKeyVault(
            new Uri($"https://{builder.Configuration["VaultName"]}.vault.azure.net/"),
            new DefaultAzureCredential());
    }

    builder.Host.UseSerilog();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

    services.ConfigureMSGraphComponent(builder.Configuration);
    var graphConfig = new GraphConfiguration();
    builder.Configuration.Bind("MSGraphConfig", graphConfig);
    services.AddSingleton(graphConfig);

    services.AddControllersWithViews();
    services.AddRazorPages();

    AddAuthorization(services);

    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    services.TryAddScoped<UserContext, UserContext>();
    services.TryAddScoped<IPersonRepository, PersonRepository>();
    services.TryAddScoped<IBusinessRepository, BusinessRepository>();
    services.TryAddScoped<ICustomerRepository, CustomerRepository>();
    services.TryAddScoped<IVendorRepository, VendorRepository>();
    services.TryAddScoped<IVendorInvoiceRepository, VendorInvoiceRepository>();
    services.TryAddScoped<IVendorInvoicePaymentMethodRepository, VendorInvoicePaymentMethodRepository>();
    services.TryAddScoped<IRepairOrderRepository, RepairOrderRepository>();
    services.TryAddScoped<IManufacturerRepository, ManufacturerRepository>();
    services.TryAddScoped<ISaleCodeRepository, SaleCodeRepository>();
    services.TryAddScoped<IProductCodeRepository, ProductCodeRepository>();
    services.TryAddScoped<IInventoryItemRepository, InventoryItemRepository>();
    services.TryAddScoped<IMaintenanceItemRepository, MaintenanceItemRepository>();
    services.TryAddScoped<ICreditCardRepository, CreditCardRepository>();
    services.TryAddScoped<IExciseFeeRepository, ExciseFeeRepository>();
    services.TryAddScoped<ISalesTaxRepository, SalesTaxRepository>();
    services.TryAddScoped<IMSGraphUserService, MSGraphUserService>();
    services.TryAddScoped<IVehicleRepository, VehicleRepository>();
    services.TryAddScoped<ISettingsRepository, SettingsRepository>();
    services.TryAddScoped<IEmployeeRepository, EmployeeRepository>();
    services.TryAddScoped<ISellingPriceNameRepository, SellingPriceNameRepository>();

    services.AddHealthChecks();

    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        services.AddDbContext<ApplicationDbContext>();

        // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
        var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        AddControllersWithOptions(services, isProduction: true, requireAuthenticatedUserPolicy);
    }

    if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Testing"))
    {
        AddControllersWithOptions(services, isProduction: false);
    }

    var reportsPath = Path.Combine(builder.Environment.ContentRootPath, "Reports");

    if (builder.Environment.IsEnvironment("Testing"))
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration[$"DatabaseSettings:IntegrationTestsConnectionString"])
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
            .LogTo(Console.WriteLine, LogLevel.Information));
    }

    if (builder.Environment.IsDevelopment())
    {
        // services.AddDbContext<ApplicationDbContext>();
        // Uncomment next line and comment previous line to route all requests to a single tenant database during development
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration[$"DatabaseSettings:MigrationsConnection"])
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information));

        // Added per Telerik Reporting tutorial
        services.AddControllers().AddNewtonsoftJson();
        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
        services.TryAddSingleton<IReportServiceConfiguration>(sp =>
            new ReportServiceConfiguration
            {
                // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
                ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

                // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use:
                //ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
                HostAppId = "Menominee",
                Storage = new FileStorage(),
                ReportSourceResolver = new TypeReportSourceResolver()
                    .AddFallbackResolver(
                        new UriReportSourceResolver(reportsPath))
            });
    }

    services.AddCors(options =>
    {
        options.AddPolicy("SpecificOrigins", corsBuilder =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Clients:AllowedOrigins").Get<string[]>();

            if (allowedOrigins is not null && allowedOrigins.Length > 0)
            {
                corsBuilder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod();

                Log.Information("CORS origins applied to the policy.");
            }
        });
    });

    var app = builder.Build();
    app.UseCors("SpecificOrigins");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        //app.UseMiddleware<DebugMiddleware>(); // slows performance; enable only when needed
        //app.UseMiddleware<RequestLoggingMiddleware>(); // slows performance; enable only when needed
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    if (!app.Environment.IsDevelopment())
    {
        app.UseSerilogIngestion();
        app.UseSerilogRequestLogging();
    }

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    var options = new RewriteOptions()
        .AddRedirectToHttps();
    app.UseRewriter(options);

    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                // Global exception handling. If a production exception gets thrown,
                // middleware configuration will handle sending the correct response.
                // So each and every controller method can shed their try/catch blocks.
                context.Response.StatusCode = 500;
                //logMessage = context.Response.StatusCode.ToString();
                //logger.LogError(logMessage);
                await context.Response.WriteAsync("An unexpected fault occurred. Fault logged.");
            });
        });
    }

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/healthcheck");
    });

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static void AddAuthorization(IServiceCollection services)
{
    services.AddAuthorization(authorizationOptions =>
    {
        authorizationOptions.AddPolicy(
            Policies.IsAdmin,
            Policies.AdminPolicy());

        authorizationOptions.AddPolicy(
            Policies.IsAuthenticated,
            Policies.RequireAuthenticatedUserPolicy());

        authorizationOptions.AddPolicy(
            Policies.CanManageHumanResources,
            Policies.CanManageHumanResourcesPolicy());

        authorizationOptions.AddPolicy(
            Policies.CanManageUsers,
            Policies.CanManageUsersPolicy());

        authorizationOptions.AddPolicy(
            Policies.IsFree,
            Policies.FreeUserPolicy());

        authorizationOptions.AddPolicy(
            Policies.IsOwner,
            Policies.OwnerPolicy());

        authorizationOptions.AddPolicy(
            Policies.IsPaid,
            Policies.PaidUserPolicy());

        authorizationOptions.AddPolicy(
            Policies.IsTechnician,
            Policies.TechnicianUserPolicy());
    });
}
static void AddControllersWithOptions(IServiceCollection services, bool isProduction, AuthorizationPolicy requireAuthenticatedUserPolicy = null)
{
    services.AddControllers(mvcOptions =>
        {
            if (isProduction)
            {
                mvcOptions.ReturnHttpNotAcceptable = true; // Return 406 Not Acceptible if request content type is unavailable
                mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            }
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

    // Integrate the FluentValidation library into the ASP.NET pipeline
    // such that all errors generated by the library will show up in the ModelState.
    // Even tho we specify only BusinessToAddValidator, AddValidatorsFromAssemblyContaining
    // uses reflection to find and register ALL validators in our assembly.
    services.AddFluentValidationAutoValidation();
    services.AddValidatorsFromAssemblyContaining<BusinessRequestValidator>();
}
public partial class Program { }

