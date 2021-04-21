using Menominee.Idp.Areas.Identity.Data;
using Menominee.Idp.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Menominee.Idp.Areas.Identity.IdentityHostingStartup))]
namespace Menominee.Idp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlServer(
                         context.Configuration
                        .GetConnectionString("UserDbContextConnection")));

                services.AddIdentity<ApplicationUser, IdentityRole>(
                         options =>
                         {
                             options.Password.RequiredLength = 8;
                             options.SignIn.RequireConfirmedAccount = true;
                         })
                        .AddEntityFrameworkStores<UserDbContext>()
                        .AddDefaultTokenProviders();
            });
        }
    }
}