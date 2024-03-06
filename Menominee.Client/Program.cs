using Blazored.Toast;
using FluentValidation;
using Menominee.Client;
using Menominee.Client.Components.Customers;
using Menominee.Client.Components.RepairOrders;
using Menominee.Client.Components.Settings;
using Menominee.Client.Components.Vehicles;
using Menominee.Client.Features.Contactables.Addresses;
using Menominee.Client.Features.Contactables.Businesses;
using Menominee.Client.Features.Contactables.Emails;
using Menominee.Client.Features.Contactables.Persons;
using Menominee.Client.Features.Contactables.Persons.DriversLicenses;
using Menominee.Client.Features.Contactables.Phones;
using Menominee.Client.MessageHandlers;
using Menominee.Client.Services;
using Menominee.Client.Services.Businesses;
using Menominee.Client.Services.CreditCards;
using Menominee.Client.Services.Customers;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Services.Settings;
using Menominee.Client.Services.Shared;
using Menominee.Client.Services.Taxes;
using Menominee.Client.Services.Vehicles;
using Menominee.Shared;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Persons.DriversLicenses;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureLogging(builder);

// For more information about Syncfusion license key, see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
SyncfusionLicenseProvider.RegisterLicense(
    "NTg1MzU1QDMxMzkyZTM0MmUzMGVFZWRZcnBURWU0L3NnaE1qdzlJT1h3NEx2N3ZOSmJ1RWx3aXh5SGlrVnc9");

var apiBaseUrl = ConfigureUriBuilder(builder);

Console.WriteLine($"Program.cs apiBaseAddress: {apiBaseUrl}");

builder.Services.AddHttpClient("Menominee.ServerAPI", client =>
    client.BaseAddress = apiBaseUrl)
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(serviceProvider =>
    serviceProvider.GetRequiredService<IHttpClientFactory>()
    .CreateClient("Menominee.ServerAPI"));

AddMsalAuthentication(builder);

builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddTelerikBlazor();

builder.Services.AddScoped<LocalStorage>();

builder.Services.AddTransient<MenonineeApiAuthorizationMessageHandler>();
builder.Services.AddScoped<IRepairOrderDataService, RepairOrderDataService>();

AddAuthorizationPolicies(builder);

AddHttpClients(builder, apiBaseUrl);

AddValidators(builder);

await builder.Build().RunAsync();

static void ConfigureLogging(WebAssemblyHostBuilder builder)
{
    // Use default Console Logger if no storage connection is provided
    var loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code);

    var storageConnection = builder.Configuration["app-log-storage-connection"];
    if (!string.IsNullOrWhiteSpace(storageConnection))
    {
        var storageContainerName = builder.HostEnvironment.Environment switch
        {
            "Staging" => "client-logs-staging",
            "Development" => "client-logs-dev",
            _ => "client-logs"
        };

        // Only configure Azure Blob Storage logging if a connection string is available
        loggerConfiguration.WriteTo.AzureBlobStorage(
            connectionString: storageConnection,
            storageContainerName: storageContainerName,
            storageFileName: "menominee-client-log-{yyyy}-{MM}-{dd}.txt",
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}");
    }
    else
    {
        Console.WriteLine("Warning: Azure Blob Storage connection string is not configured. Logs will not be sent to Azure Blob Storage.");
    }

    Log.Logger = loggerConfiguration.CreateLogger();

    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
}

static void AddHttpClients(WebAssemblyHostBuilder builder, Uri baseAddress)
{
    builder.Services.AddHttpClient<IUserDataService, UserDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IPersonDataService, PersonDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICustomerDataService, CustomerDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IBusinessDataService, BusinessDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IVendorDataService, VendorDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IVendorInvoiceDataService, VendorInvoiceDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IVendorInvoicePaymentMethodDataService, VendorInvoicePaymentMethodDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IRepairOrderDataService, RepairOrderDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IManufacturerDataService, ManufacturerDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ISaleCodeDataService, SaleCodeDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IProductCodeDataService, ProductCodeDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IInventoryItemDataService, InventoryItemDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IMaintenanceItemDataService, MaintenanceItemDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICreditCardDataService, CreditCardDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IExciseFeeDataService, ExciseFeeDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ISalesTaxDataService, SalesTaxDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ISettingDataService, SettingDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IVehicleDataService, VehicleDataService>(
        client => client.BaseAddress = baseAddress)
        .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();
}

static Uri ConfigureUriBuilder(WebAssemblyHostBuilder builder)
{
    builder.Services.Configure<UriBuilderConfiguration>(builder.Configuration.GetSection(nameof(UriBuilderConfiguration)));
    builder.Services.AddSingleton<UriBuilderFactory>();

    var uriBuilderConfig = builder.Configuration.GetSection("UriBuilderConfiguration");
    var scheme = uriBuilderConfig["Scheme"];
    var host = uriBuilderConfig["Host"];
    var port = uriBuilderConfig["Port"];

    return port is not null
                      ? new Uri($"{scheme}://{host}:{port}/")
                      : new Uri($"{scheme}://{host}/");
}

static void AddAuthorizationPolicies(WebAssemblyHostBuilder builder)
{
    builder.Services.AddAuthorizationCore(authorizationOptions =>
    {
        authorizationOptions.AddPolicy(
            Policies.IsAdmin,
            // policy => policy.Requirements.Add(new CustomAuthorization(new[] { ShopRole.Admin.ToString(), ShopRole.SuperAdmin.ToString() })));
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

static void AddMsalAuthentication(WebAssemblyHostBuilder builder)
{
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

        var accessTokenScopeUrl = builder.Configuration["AzureAdB2C:AccessTokenScopeUrl"];
        if (!string.IsNullOrEmpty(accessTokenScopeUrl))
        {
            options.ProviderOptions.DefaultAccessTokenScopes.Add(accessTokenScopeUrl);
        }
    });
}

static void AddValidators(WebAssemblyHostBuilder builder)
{
    // Manually register validators to preserve performance; prevent scanning assemblies
    // via component declaration: <FluentValidationValidator DisableAssemblyScanning="@true" />
    builder.Services.AddTransient<IValidator<CustomerToWrite>, CustomerRequestValidator>();
    builder.Services.AddTransient<IValidator<PersonToWrite>, PersonRequestValidator>();
    builder.Services.AddTransient<IValidator<PersonNameToWrite>, PersonNameRequestValidator>();
    builder.Services.AddTransient<IValidator<DriversLicenseToWrite>, DriversLicenseRequestValidator>();
    builder.Services.AddTransient<IValidator<AddressToWrite>, AddressRequestValidator>();
    builder.Services.AddTransient<IValidator<PhoneToWrite>, PhoneRequestValidator>();
    builder.Services.AddTransient<IValidator<EmailToWrite>, EmailRequestValidator>();
    builder.Services.AddTransient<IValidator<List<PhoneToWrite>>, PhonesRequestValidator>();
    builder.Services.AddTransient<IValidator<List<EmailToWrite>>, EmailsRequestValidator>();
    builder.Services.AddTransient<IValidator<BusinessToWrite>, BusinessRequestValidator>();
    builder.Services.AddTransient<IValidator<BusinessNameRequest>, BusinessNameRequestValidator>();
    builder.Services.AddTransient<IValidator<List<VehicleToWrite>>, VehiclesRequestValidator>();
    builder.Services.AddTransient<IValidator<VendorInvoicePaymentMethodRequest>, VendorInvoicePaymentMethodRequestValidator>();
}