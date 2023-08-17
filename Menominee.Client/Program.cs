using Blazored.Toast;
using Menominee.Shared;
using Menominee.Client;
using Menominee.Client.Components.RepairOrders;
using Menominee.Client.MessageHandlers;
using Menominee.Client.Services;
using Menominee.Client.Services.CreditCards;
using Menominee.Client.Services.Customers;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Services.Taxes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using Menominee.Client.Services.Businesses;
using Menominee.Client.Services.Settings;

// Add your Syncfusion license key for Blazor platform with corresponding Syncfusion NuGet version referred in project. For more information about license key see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ3MzAyQDMxMzkyZTMzMmUzMGF5MU1kSEI2RnZMQWMxR3dqSlM4T2MvVFBWTFdBbEhzckF2TVJwSVlJVTQ9");
SyncfusionLicenseProvider.RegisterLicense(
    "NTg1MzU1QDMxMzkyZTM0MmUzMGVFZWRZcnBURWU0L3NnaE1qdzlJT1h3NEx2N3ZOSmJ1RWx3aXh5SGlrVnc9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Menominee.ServerAPI"
        , client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Menominee.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://JancoDEVB2C.onmicrosoft.com/cvmapi/API.Access");
});

builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddTelerikBlazor();

builder.Services.AddScoped<LocalStorage>();
builder.Services.AddScoped<IUserDataService, UserDataService>();

builder.Services.AddTransient<MenonineeApiAuthorizationMessageHandler>();
builder.Services.AddScoped<IRepairOrderDataService, RepairOrderDataService>();

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

//var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

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

await builder.Build().RunAsync();
