using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using Microsoft.Extensions.Logging;
using Menominee.UiExperiments.MessageHandlers;
using Menominee.UiExperiments.Services;
using Microsoft.Extensions.Configuration;

namespace Menominee.UiExperiments
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Add your Syncfusion license key for Blazor platform with corresponding Syncfusion NuGet version referred in project. For more information about license key see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDU2NDQ2QDMxMzkyZTMxMmUzMFhtMXY2TG1wdTFsc1RlQmpvTXV4NEhWMisrTmEyUEZ3TVhPeEpqaEFGcGc9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.AddTransient<MenonineeApiAuthorizationMessageHandler>();

            var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));

            builder.Services.AddHttpClient<IPersonDataService, PersonDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();
            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }
    }
}
