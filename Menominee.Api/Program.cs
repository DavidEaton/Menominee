using Azure.Identity;
using Menominee.Api;
using Menominee.Api.Businesses;
using Menominee.Api.CreditCards;
using Menominee.Api.Customers;
using Menominee.Api.Data;
using Menominee.Api.Employees;
using Menominee.Api.Inventory;
using Menominee.Api.Manufacturers;
using Menominee.Api.Payables.Invoices;
using Menominee.Api.Payables.PaymentMethods;
using Menominee.Api.Payables.Vendors;
using Menominee.Api.Persons;
using Menominee.Api.ProductCodes;
using Menominee.Api.RepairOrders;
using Menominee.Api.SaleCodes;
using Menominee.Api.SellingPriceNames;
using Menominee.Api.Settings;
using Menominee.Api.Taxes;
using Menominee.Api.Users;
using Menominee.Api.Vehicles;
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
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(new JsonFormatter(), @"menominee-api-log-.json", rollingInterval: RollingInterval.Day)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;

    IdentityModelEventSource.ShowPII = builder.Environment.IsDevelopment();

    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    {
        builder.Configuration.AddAzureKeyVault(
            new Uri($"https://{builder.Configuration["VaultName"]}.vault.azure.net/"),
            new DefaultAzureCredential());
    }

    builder.Host.UseSerilog();

    // Add services to the container.
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

    services.ConfigureMSGraphComponent(builder.Configuration);
    var graphConfig = new GraphConfiguration();
    builder.Configuration.Bind("MSGraphConfig", graphConfig);
    services.AddSingleton(graphConfig);

    services.AddControllersWithViews();
    services.AddRazorPages();

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
    services.AddCors(o => o.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }));

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
        AddControllersWithOptions(services, isProduction: false);

    var reportsPath = Path.Combine(builder.Environment.ContentRootPath, "Reports");

    if (builder.Environment.IsEnvironment("Testing"))
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration[$"DatabaseSettings:IntegrationTestsConnectionString"])
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
            .LogTo(Console.WriteLine, LogLevel.Information));

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

    var app = builder.Build();
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

    app.UseCors();
    // TODO: check how this should be best applied in relation to the use setup above...
    // app.UseCors(cors => cors.WithOrigins(builder.Configuration.GetSection($"Clients:AllowedOrigins").Get<string>().Split(";"))
    //     .AllowAnyMethod()
    //     .AllowAnyHeader()
    // //    .WithHeaders(HeaderNames.ContentType)
    // );

    var options = new RewriteOptions()
        .AddRedirectToHttps();
    app.UseRewriter(options);

    if (builder.Environment.IsProduction())
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
    //.AddFluentValidation(options =>
    //{
    //    // This method integrates the FluentValidation library into the ASP.NET pipeline
    //    // such that all errors generated by the library will show up in the ModelState.
    //    // Even tho we specify only BusinessToAddValidator, RegisterValidatorsFromAssemblyContaining
    //    // uses reflection to find and register ALL validators in our assembly.
    //    options.RegisterValidatorsFromAssemblyContaining<BusinessValidator>();
    //});
}
public partial class Program { }