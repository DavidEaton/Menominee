using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
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
            const string Connection = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
            const string TestConnection = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";
            const bool useConsoleLoggerInTest = true;
            string environment = HostEnvironment.EnvironmentName;
            string api_env = Environment.GetEnvironmentVariable("API_ENVIRONMENT");
            // An environment variable is setup in Azure.  If it's set then grab the connection string
            // from the Azure slot, otherwise use the appsetting.json connection string.

            IdentityModelEventSource.ShowPII = HostEnvironment.IsDevelopment();

            if (environment == "Production")
                services
                    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                        {
                            options.Authority = Configuration[$"IDPSettings:BaseUrl:{environment}"];
                            options.ApiName = Configuration["ApiName"];
                        })
                    .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                        options =>
                         {
                             options.Authority = Configuration[$"IDPSettings:BaseUrl:{environment}"];
                             options.Audience = Configuration["ApiName"];
                             options.RequireHttpsMetadata = false;
                             options.TokenValidationParameters = new
                             TokenValidationParameters()
                             {
                                 ValidateAudience = false
                             };
                         });

            if (environment != "Testing")
                services.AddScoped(_ => new AppDbContext(Connection, HostEnvironment.IsDevelopment()));

            if (environment == "Testing")
                services.AddScoped(_ => new AppDbContext(TestConnection, useConsoleLoggerInTest));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddAutoMapper(typeof(Startup));
            services.AddCors();
            services.AddHealthChecks();

            // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
            if (environment == "Production")
            {
                var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                services.AddControllers(mvcOptions =>
                {
                    // Return 406 Not Acceptible if request content type is unavailable
                    mvcOptions.ReturnHttpNotAcceptable = true;
                    // Only allow authenticated users
                    mvcOptions.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
                    // Provide xml content type
                }).AddXmlDataContractSerializerFormatters()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                  });
            }

            if (HostEnvironment.IsDevelopment())
                services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            string environment = HostEnvironment.EnvironmentName;
            string[] testOrigin = { "http://localhost:44378, https://localhost:44378" };

            app.UseHttpsRedirection();
            app.UseRouting();

            if (!HostEnvironment.IsDevelopment())
            {

                app.UseAuthorization();
                app.UseAuthentication();
                app.UseCors(cors => cors.WithOrigins(Configuration.GetSection($"Clients:Origins:{environment}").Get<string>())
                                        .AllowAnyMethod().AllowAnyHeader());
            }

            if (environment != "Prodution")
                app.UseCors(cors => cors.WithOrigins(testOrigin)
                                    .AllowAnyMethod().AllowAnyHeader());

            if (environment == "Prodution")
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
