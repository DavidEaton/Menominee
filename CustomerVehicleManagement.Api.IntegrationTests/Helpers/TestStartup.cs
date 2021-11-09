using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.LoginPath = new PathString("/auth/login");
                            options.AccessDeniedPath = new PathString("/auth/denied");
                        });

            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    "RequireAuthenticatedUserPolicy",
                    requireAuthenticatedUserPolicy);

                authorizationOptions.AddPolicy(
                    Policies.AdminOnly,
                    Policies.AdminPolicy());

                authorizationOptions.AddPolicy(
                    Policies.CanManageHumanResources,
                    Policies.CanManageHumanResourcesPolicy());

                authorizationOptions.AddPolicy(
                    Policies.CanManageUsers,
                    Policies.CanManageUsersPolicy());

                authorizationOptions.AddPolicy(
                    Policies.FreeUser,
                    Policies.FreeUserPolicy());

                authorizationOptions.AddPolicy(
                    Policies.OwnerOnly,
                    Policies.OwnerPolicy());

                authorizationOptions.AddPolicy(
                    Policies.PaidUser,
                    Policies.PaidUserPolicy());

                authorizationOptions.AddPolicy(
                    Policies.TechniciansUser,
                    Policies.TechnicianUserPolicy());
            });

            services.AddControllers(mvcOptions =>
            {
                mvcOptions.ReturnHttpNotAcceptable = true; // Return 406 Not Acceptible if request content type is unavailable
                mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}