using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Inventory;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Payables.Invoices;
using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.ProductCodes;
using CustomerVehicleManagement.Api.RepairOrders;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Api.Users;
using CustomerVehicleManagement.Data;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Validators;
using FluentValidation.AspNetCore;
using Janco.Idp.Areas.Identity.Data;
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

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Authority = Configuration[$"IDPSettings:BaseUrl"];// base-address of our identity server
                        options.Audience = Configuration["ApiName"]; // name of the API resource
                        options.RequireHttpsMetadata = HostEnvironment.IsProduction() ? true : false;
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

            services.AddDbContext<IdentityUserDbContext>(options =>
                                                         options
                                                        .UseSqlServer(Configuration.GetConnectionString("Idp"))
                                                        );

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<IdentityUserDbContext>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // webHostBuilder.ConfigureServices in Test adds mock services
            // TryAddScoped won't re-add or overwrite services already added
            // to the container, but AddScoped will.
            services.TryAddScoped<UserContext, UserContext>();
            services.TryAddScoped<IPersonRepository, PersonRepository>();
            services.TryAddScoped<IOrganizationRepository, OrganizationRepository>();
            services.TryAddScoped<ICustomerRepository, CustomerRepository>();
            services.TryAddScoped<IVendorRepository, VendorRepository>();
            services.TryAddScoped<IVendorInvoiceRepository, VendorInvoiceRepository>();
            services.TryAddScoped<IRepairOrderRepository, RepairOrderRepository>();
            services.TryAddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.TryAddScoped<ISaleCodeRepository, SaleCodeRepository>();
            services.TryAddScoped<IProductCodeRepository, ProductCodeRepository>();
            services.TryAddScoped<IInventoryItemRepository, InventoryItemRepository>();

            services.AddHealthChecks();
            services.AddCors();

            if (HostEnvironment.IsProduction())
            {
                services.AddDbContext<ApplicationDbContext>();

                // All controller actions which are not marked with [AllowAnonymous] will require that the user is authenticated.
                AddControllersWithOptions(services, true, requireAuthenticatedUserPolicy);
            }

            if (HostEnvironment.IsDevelopment())
            {
                AddControllersWithOptions(services, false);
                services.AddDbContext<ApplicationDbContext>();
                // Uncomment next line and comment previous line to route all requests to a single tenant database during development
                //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration[$"DatabaseSettings:MigrationsConnection"]));
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
                options.RegisterValidatorsFromAssemblyContaining<OrganizationValidator>();
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
                        // So each and every controller method can shed their try/catch blocks.
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
