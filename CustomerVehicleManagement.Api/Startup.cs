using AutoMapper;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace CustomerVehicleManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            const string CONNECTION = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
            string environment = HostEnvironment.IsDevelopment() ? "Development" : "Production";
            string api_env = Environment.GetEnvironmentVariable("API_ENVIRONMENT");
            // An environment variable is setup in Azure.  If it's set then grab the connection string
            // from the Azure slot, otherwise use the appsetting.json connection string.

            // ONLY USE IN DEVELOPMENT
            //IdentityModelEventSource.ShowPII = true;

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        // Authority: the base-address of our IDP
            //        options.Authority = Configuration[$"IDPSettings:BaseUrl:{environment}"];
            //        options.ApiName = Configuration["ApiName"];
            //    });

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(CONNECTION));
            //services.AddDbContext<IdentityUserDbContext>(options =>
            //                                             options
            //                                             // Connect to our IDP
            //                                             .UseSqlServer(Configuration[$"IDPSettings:Connection:{environment}"]));
            //services.AddScoped<UserContext, UserContext>();
            //services.AddScoped<IdentityUserDbContext, IdentityUserDbContext>();

            // IHttpContextAccessor is no longer wired up by default, so register it
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors();
            services.AddHealthChecks();

            // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddControllers(mvcOptions => 
            {
                // Return 406 Not Acceptible if request content type is unavailable
                mvcOptions.ReturnHttpNotAcceptable = true;
                //mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
        }).AddXmlDataContractSerializerFormatters(); // Provide xml content type
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            string environment = HostEnvironment.IsDevelopment() ? "Development" : "Production";

            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();
            //app.UseAuthentication();
            //app.UseCors(cors => cors.WithOrigins(Configuration.GetSection("Clients:Origins").Get<List<string>>()).AllowAnyMethod().AllowAnyHeader());
            //app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:Origins:{environment}").Get<string>())
            //                        .AllowAnyMethod().AllowAnyHeader());
            app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:Origins").Get<string>())
                                    .AllowAnyMethod().AllowAnyHeader());

            if (HostEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            else
            {
                // Production || Testing
                string api_env = Environment.GetEnvironmentVariable("API_ENVIRONMENT");
                var logMessage = $"Configure called in {api_env} slot";

                var options = new RewriteOptions()
                    .AddRedirectToHttps();
                app.UseRewriter(options);

                // Global exception handling - if a production exception gets thrown, middleware configuration will handle sending the correct response:
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;

                        await context.Response.WriteAsync("An unexpected fault occurred. Fault logged.");
                    });
                });

                // Testing 
                if (api_env == "Testing")
                    app.UseDeveloperExceptionPage();

            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}
