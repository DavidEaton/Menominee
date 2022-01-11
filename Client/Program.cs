using MenomineePlayWASM.Client.Helpers;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MenomineePlayWASM.Shared.Services.Payables.Vendors;
using MenomineePlayWASM.Shared.Services.Payables.Invoices;
using MenomineePlayWASM.Shared.Services.Inventory;
using MenomineePlayWASM.Shared.Services.RepairOrders;
//using CurrieTechnologies.Razor.SweetAlert2;

namespace MenomineePlayWASM.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Add your Syncfusion license key for Blazor platform with corresponding Syncfusion NuGet version referred in project. For more information about license key see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc4MjUyQDMxMzgyZTM0MmUzMGxUQTdBU2NOZ3lmdys0dmVjYXhPL2N5cElKaWRuZVR1MkNVTHIyTHdxWUk9");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDU2NDQ2QDMxMzkyZTMxMmUzMFhtMXY2TG1wdTFsc1RlQmpvTXV4NEhWMisrTmEyUEZ3TVhPeEpqaEFGcGc9");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDY2NDk2QDMxMzkyZTMyMmUzMGlFcUdGUVpyR0ZtbGEvNWMwemZObVd6ZEJtTFl5QUNFSVhhNnc5MGJCWlE9");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTIzNTkyQDMxMzkyZTMzMmUzMGgzcEk0cFJoWDlaNC8vaURoT25sNXZXdjdEUXhtdW5ZWjdmcU5OL0JFSDg9");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ3MzAyQDMxMzkyZTMzMmUzMGF5MU1kSEI2RnZMQWMxR3dqSlM4T2MvVFBWTFdBbEhzckF2TVJwSVlJVTQ9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDisplayMessage, DisplayMessage>();
            //services.AddScoped<IHttpService, HttpService>();

            // Inventory
            services.AddScoped<IInventoryItemDataService, InventoryItemDataService>();

            // Repair Orders
            services.AddScoped<IRepairOrderDataService, RepairOrderDataService>();

            // Payables
            //services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorDataService, VendorDataService>();
            services.AddScoped<IVendorInvoiceDataService, VendorInvoiceDataService>();
            //services.AddScoped<IVendorInvoiceRepository, VendorInvoiceRepository>();
            //services.AddScoped<IVendorInvoiceItemRepository, VendorInvoiceItemRepository>();
            //services.AddScoped<IVendorInvoicePaymentRepository, VendorInvoicePaymentRepository>();
            //services.AddScoped<IVendorInvoicePaymentMethodRepository, VendorInvoicePaymentMethodRepository>();
            //services.AddScoped<IVendorInvoiceTaxRepository, VendorInvoiceTaxRepository>();
            //services.AddScoped<ICreditReturnRepository, CreditReturnRepository>();

            //// Customers
            //services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddSyncfusionBlazor();
            services.AddBlazoredModal();
            //services.AddSweetAlert2();
            services.AddTelerikBlazor();
            services.AddBlazoredToast();
        }

    }
}
