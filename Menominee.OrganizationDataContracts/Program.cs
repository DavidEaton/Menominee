using Blazored.Toast;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Validators;
using FluentValidation;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menominee.OrganizationDataContracts
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddTelerikBlazor();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.AddBlazoredToast();
            builder.Services.AddScoped<LocalStorage>();
            var baseAddress = new Uri("https://localhost:54382/api/");

            // HTTPClient
            builder.Services.AddHttpClient<IOrganizationDataService, OrganizationDataService>
                                          (client => client.BaseAddress = baseAddress);
            // Validation
            builder.Services.AddTransient<IValidator<OrganizationToWrite>, OrganizationValidator>();

            await builder.Build().RunAsync();
        }
    }
}
