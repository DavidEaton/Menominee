using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Users;
using CustomerVehicleManagement.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = HostEnvironment.IsDevelopment();

            // TODO: Separate Startup class for tests should use:
            //if (HostEnvironment.IsDevelopment())
            //    services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        // base-address of our identityserver
                        options.Authority = Configuration[$"IDPSettings:BaseUrl"];

                        // name of the API resource
                        options.Audience = Configuration["ApiName"];

                        options.RequireHttpsMetadata = false;
                        //options.ApiSecret
                        options.TokenValidationParameters = new
                        TokenValidationParameters()
                        {
                            ValidateAudience = false
                        };
                    })
                ;

            // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("RequireAuthenticatedUserPolicy", requireAuthenticatedUserPolicy);

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

            services.AddDbContext<IdentityUserDbContext>(options =>
                                                         options
                                                        .UseSqlServer(Configuration[$"IDPSettings:Connection"]));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<UserContext, UserContext>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddHealthChecks();
            services.AddCors();

            services.AddControllers(mvcOptions =>
            {
                mvcOptions.ReturnHttpNotAcceptable = true; // Return 406 Not Acceptible if request content type is unavailable
                mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            }).AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:AllowedOrigins").Get<string>().Split(";"))
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());

            if (HostEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

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
                        //logMessage = context.Response.StatusCode.ToString();
                        //logger.LogError(logMessage);
                        await context.Response.WriteAsync("An unexpected fault occurred. Fault logged.");
                    });
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}
