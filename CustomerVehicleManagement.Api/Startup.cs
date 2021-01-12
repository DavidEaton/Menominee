using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Repositories;
using CustomerVehicleManagement.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string environment = HostEnvironment.IsDevelopment() ? "Development" : "Production";

            services.AddDbContext<AppDbContext>();
            // IHttpContextAccessor is no longer wired up by default, so register it
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson(options =>
                                                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            string environment = HostEnvironment.IsDevelopment() ? "Development" : "Production";

            app.UseRouting();
            //app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:Origins").Get<string>())
            //                        .AllowAnyMethod().AllowAnyHeader());

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
            });
        }
    }
}
