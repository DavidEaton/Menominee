using Menominee.Tests.Integration.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Menominee.Tests.Integration
{
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureKestrel(options =>
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    // Configure your HTTPS settings here, for example, you can set the SSL certificate.
                });
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IDataSeeder, DataSeeder>();
            });

            base.ConfigureWebHost(builder);
        }
    }
}
