using CustomerVehicleManagement.Api.User;
using CustomerVehicleManagement.Data.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel;
using SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api
{
    /// <summary>
    /// This DbContext is used by the api, injected into the services Inversion of Control (IoC) container during Startup
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly bool useConsoleLogger;
        private readonly string connection;
        private readonly UserContext userContext;
        //private readonly IdentityUserDbContext identityContext;
        private List<Tenant> Tenants;
        public IWebHostEnvironment environment;


        private IConfiguration configuration { get; }

        protected AppDbContext() { }

        public AppDbContext(string connection, bool useConsoleLogger)
        {
            this.connection = connection;
            this.useConsoleLogger = useConsoleLogger;
        }

        public AppDbContext(string connection,
                            bool useConsoleLogger,
                            UserContext userContext,
                            //IdentityUserDbContext identityContext,
                            IWebHostEnvironment environment,
                            IConfiguration configuration)
        {
            this.connection = connection;
            this.useConsoleLogger = useConsoleLogger;
            this.userContext = userContext;
            //this.identityContext = identityContext;
            this.environment = environment;
            this.configuration = configuration;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Unit tests will configure context with test provider
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                      .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                      .AddConsole();
                });

                optionsBuilder.UseSqlServer(connection)
                              .UseLazyLoadingProxies(false);

                if (useConsoleLogger)
                {
                    optionsBuilder
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging();
                }

                //return;
            }

            //Tenant tenant = GetTenant(GetTenantId(httpContext));

            //if (!environment.IsEnvironment("Testing"))
            //{
            //    Tenant tenant = GetTenant(GetTenantId(userContext));

            //    if (tenant != null)
            //    {
            //        //string errorMessage = "Unable to find tenant.";
            //        //Log.Error(errorMessage);
            //        //throw new ApplicationException(errorMessage);


            //        var dbConnectionOptions = new DbConnectionOptions
            //        {
            //            DatabaseName = tenant.Name,
            //            Server = configuration["DatabaseSettings:Server:Name"],
            //            IntegratedSecurity = environment.IsDevelopment(),
            //            Password = configuration["DatabaseSettings:Server:Password"],
            //            UserId = configuration["DatabaseSettings:Server:UserName"],
            //            TrustServerCertificate = environment.IsDevelopment()
            //        };

            //        optionsBuilder.UseSqlServer(BuildTenantDbConnectionString(dbConnectionOptions));

            //        base.OnConfiguring(optionsBuilder);
            //    }
            //}



        }

        private string BuildTenantDbConnectionString(DbConnectionOptions dbOptions)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dbOptions.Server,
                InitialCatalog = dbOptions.DatabaseName,
                IntegratedSecurity = dbOptions.IntegratedSecurity,
                Password = dbOptions.Password,
                UserID = dbOptions.UserId,
                PersistSecurityInfo = dbOptions.PersistSecurityInfo,
                MultipleActiveResultSets = dbOptions.MultipleActiveResultSets,
                Encrypt = dbOptions.Encrypt,
                TrustServerCertificate = dbOptions.TrustServerCertificate,
                ConnectTimeout = dbOptions.ConnectTimeout
            };

            return builder.ConnectionString;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Ignore<Entity>();

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
        }

        private Tenant GetTenant(string tenantId)
        {
            //Tenants = identityContext.Tenants.ToList();

            //if (Tenants != null)
            //    return Tenants.Find(t => t.Id.ToString().ToLower() == tenantId.ToLower());

            return null;
        }

        private static string GetTenantId(UserContext UserContext)
        {
            if (UserContext == null)
                return string.Empty;

            var claims = UserContext.Claims;
            var tenantId = string.Empty;

            try
            {
                tenantId = claims.First(c => c.Type == "tenantId").Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from GetTenantId(): {ex.Message}");
                return string.Empty;
            }

            return tenantId;
        }

        private static string GetTenantName(UserContext UserContext)
        {
            if (UserContext == null)
                return string.Empty;

            var claims = UserContext.Claims;
            var tenantName = string.Empty;

            try
            {
                tenantName = claims.First(c => c.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from GetTenantName(): {ex.Message}");
                return string.Empty;
            }

            return tenantName;
        }
    }

    internal class DbConnectionOptions
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool PersistSecurityInfo = false;
        public bool MultipleActiveResultSets = false;
        public bool Encrypt = true;
        public bool TrustServerCertificate { get; set; }
        public int ConnectTimeout = 30;
    }
}
