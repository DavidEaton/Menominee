using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using Menominee.UiExperiments;

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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }
    }
}
