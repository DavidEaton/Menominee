using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Handlers;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Users;
using CustomerVehicleManagement.Api.Validators;
using CustomerVehicleManagement.Shared;
using FluentValidation.AspNetCore;
using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;

namespace CustomerVehicleManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
                       IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = HostEnvironment.IsDevelopment();

            if (HostEnvironment.IsDevelopment())
                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();

            if (HostEnvironment.IsProduction())
            {

                services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                        options =>
                         {
                             options.Authority = Configuration[$"IDPSettings:BaseUrl"];
                             options.Audience = Configuration["ApiName"];
                             options.RequireHttpsMetadata = false;
                             options.TokenValidationParameters = new
                             TokenValidationParameters()
                             {
                                 ValidateAudience = false
                             };
                         })
                    ;

                services.AddAuthorization(authorizationOptions =>
                {
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

            }

            services.AddDbContext<IdentityUserDbContext>(options =>
                                                         options
                                                        .UseSqlServer(Configuration[$"IDPSettings:Connection"]));




            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityUserDbContext>()
            .AddDefaultTokenProviders();



            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // webHostBuilder.ConfigureServices in Test adds mock services
            // TryAddScoped won't re-add or overwrite services already added
            // to the container, but AddScoped will.
            services.TryAddScoped<UserContext, UserContext>();
            services.AddDbContext<ApplicationDbContext>();
            services.TryAddScoped<IPersonRepository, PersonRepository>();
            services.TryAddScoped<IOrganizationRepository, OrganizationRepository>();
            services.TryAddScoped<ICustomerRepository, CustomerRepository>();

            services.AddHealthChecks();
            services.AddCors();

            if (HostEnvironment.IsProduction())
            {
                // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
                var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                AddControllersWithOptions(services, true, requireAuthenticatedUserPolicy);
            }

            if (HostEnvironment.IsDevelopment())
            {
                AddControllersWithOptions(services, false);
            }
        }

        private static void AddControllersWithOptions(IServiceCollection services, bool isProduction, AuthorizationPolicy requireAuthenticatedUserPolicy = null)
        {
            services.AddControllers(mvcOptions =>
            {
                if (isProduction)
                {
                    mvcOptions.ReturnHttpNotAcceptable = true; // Return 406 Not Acceptible if request content type is unavailable
                    mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
                }
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            })
            .AddFluentValidation(options =>
            {
                // This method integrates the FluentValidation library into the ASP.NET pipeline
                // such that all errors generated by the library will show up in the ModelState.
                // Even tho we specify OrganizationToAddValidator, RegisterValidatorsFromAssemblyContaining
                // uses reflection to find and register all validators in our assembly.
                options.RegisterValidatorsFromAssemblyContaining<OrganizationToAddValidator>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            if (HostEnvironment.IsProduction())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:Origins").Get<string>())
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());

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

                if (HostEnvironment.IsProduction())
                {
                    app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        // Global exception handling. If a production exception gets thrown,
                        // middleware configuration will handle sending the correct response.
                        // So each and every controller method can shed their try/catch blocks:
                        //    try...catch (Exception ex)
                        //      return StatusCode(StatusCodes.Status500InternalServerError, ex);
                        context.Response.StatusCode = 500;
                        logMessage = context.Response.StatusCode.ToString();
                        //logger.LogError(logMessage);
                        await context.Response.WriteAsync("An unexpected fault occurred. Fault logged.");
                    });
                });
                }

                // Staging
                if (api_env == "Staging")
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
