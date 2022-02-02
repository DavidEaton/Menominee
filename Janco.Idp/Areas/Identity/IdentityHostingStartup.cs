using Janco.Idp.Areas.Identity.Data;
using Janco.Idp.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: HostingStartup(typeof(Janco.Idp.Areas.Identity.IdentityHostingStartup))]
namespace Janco.Idp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        private string Connection;

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var Environment = context.Configuration.GetValue<string>("ApplicationSettings:Environment");

                if (Environment == "Development")
                    Connection = context.Configuration.GetConnectionString("UserDbContextConnection");

                if (Environment == "Production")
                {
                    var builder = new SqlConnectionStringBuilder
                    {
                        DataSource = context.Configuration.GetValue<string>("Database:DataSource"),
                        UserID = context.Configuration.GetValue<string>("Database:UserId"),
                        Password = context.Configuration.GetValue<string>("Database:Password"),
                        InitialCatalog = context.Configuration.GetValue<string>("Database:InitialCatalog"),
                    };

                    Connection = new SqlConnection(builder.ConnectionString).ConnectionString;
                }

                Log.Logger.Information($"Environment: {Environment}");

                services.AddDbContext<UserDbContext>(options =>
                    options.UseSqlServer(Connection));

                services.AddIdentity<ApplicationUser, IdentityRole>(
                         options =>
                         {
                             options.Password.RequiredLength = int.Parse(context.Configuration.GetValue<string>("ApplicationSettings:MinimumPasswordLength"));
                             options.SignIn.RequireConfirmedAccount = true;
                         })
                        .AddEntityFrameworkStores<UserDbContext>()
                        .AddDefaultTokenProviders();
            });
        }
    }
}