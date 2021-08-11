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
        private readonly UserContext userContext;
        private string connection;
        //private readonly IdentityUserDbContext identityContext;
        private List<Tenant> Tenants;
        public IWebHostEnvironment environment;
        const string ConnectionDevelopment = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
        const string ConnectionTest = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";
        //const bool useConsoleLoggerInTest = true;


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

                if (connection == null)
                    connection = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";

                optionsBuilder.UseSqlServer(connection)
                                                  .UseLazyLoadingProxies(false);

                if (useConsoleLogger)
                {
                    optionsBuilder
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging();
                }
            }


            if (environment.IsDevelopment())
            {
                connection = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
            }

            if (environment.IsProduction())
            {
                connection = GetTenantConnection();
            }
            if (environment.IsEnvironment("Testing"))
            {
                connection = "Server=localhost;Database=MenomineeTest;Trusted_Connection=True;";
            }

            optionsBuilder.UseSqlServer(connection);
        }

        private string GetTenantConnection()
        {
            string tenantName = GetTenantName(userContext);

            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                var connectionOptions = new DatabaseConnectionOptions
                {
                    DatabaseName = tenantName,
                    Server = configuration["DatabaseSettings:Server:Name"],
                    IntegratedSecurity = environment.IsDevelopment(),
                    Password = configuration["DatabaseSettings:Server:Password"],
                    UserId = configuration["DatabaseSettings:Server:UserName"],
                    TrustServerCertificate = environment.IsDevelopment()
                };

                return BuildTenantDbConnectionString(connectionOptions);
            }

            return string.Empty;
        }

        private string BuildTenantDbConnectionString(DatabaseConnectionOptions dbOptions)
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
}
