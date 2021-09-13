using CustomerVehicleManagement.Shared;
using Menominee.Client.MessageHandlers;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Syncfusion.Blazor;
using System;
using System.Threading.Tasks;

namespace Menominee.Client
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
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddTransient<MenonineeApiAuthorizationMessageHandler>();


            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
                builder.Configuration.Bind("UserOptions", options.UserOptions);
            });

            builder.Services.AddAuthorizationCore(authorizationOptions =>
            {

                authorizationOptions.AddPolicy(
                    Policies.CanManageUsers,
                    Policies.CanManageUsersPolicy());

                    //policyBuilder =>
                    //{
                    //    policyBuilder.RequireAuthenticatedUser();
                    //    //policyBuilder.RequireClaim(ClaimType.ShopRole.ToString(), ShopRole.HumanResources.ToString());
                    //    policyBuilder.RequireRole(ShopRole.Admin.ToString());
                    //});

                //authorizationOptions.AddPolicy(
                //    "PaidSubscriptionCanDoStuff",
                //    policyBuilder =>
                //    {
                //        policyBuilder.RequireAuthenticatedUser();
                //        policyBuilder.RequireClaim("subscriptionLevel", "Paid");
                //    });

                //authorizationOptions.AddPolicy(
                //    Policies.CanManageUsers,
                //    Policies.CanManageUsersPolicy()
                //    );
            });

            await builder.Build().RunAsync();

            var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl"));

            builder.Services.AddHttpClient<IUserDataService, UserDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IPersonDataService, PersonDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<ICustomerDataService, CustomerDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IOrganizationDataService, OrganizationDataService>(
                client => client.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MenonineeApiAuthorizationMessageHandler>();


        }

        private static void ConfigureLogging(
          WebAssemblyHostBuilder builder,
          string section = "Logging")
        {
            builder.Logging.AddConfiguration(builder.Configuration.GetSection(section));
        }
    }
}
