using CustomerVehicleManagement.Api.User;
using CustomerVehicleManagement.Data.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernel;
using SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment environment;
        private readonly UserContext userContext;
        //private readonly IdentityUserDbContext identityContext;
        private List<Tenant> tenants;
        readonly ILogger<ApplicationDbContext> logger;
        private string connection;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            UserContext userContext,
            //IdentityUserDbContext identityContext,
            ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            this.environment = environment;
            this.Configuration = configuration;
            this.userContext = userContext;
            //this.identityContext = identityContext;
            this.logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            connection = GetTenantConnection();

            options.UseSqlServer(connection);
            base.OnConfiguring(options);
        }

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

        private string GetTenantConnection()
        {
            string tenantName = GetTenantName(userContext);

            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                var connectionOptions = new DatabaseConnectionOptions
                {
                    DatabaseName = tenantName,
                    Server = Configuration["DatabaseSettings:Server:Name"],
                    IntegratedSecurity = environment.EnvironmentName == "Development",
                    Password = Configuration["DatabaseSettings:Server:Password"],
                    UserId = Configuration["DatabaseSettings:Server:UserName"],
                    TrustServerCertificate = environment.EnvironmentName == "Development"
                };

                return BuildConnectionString(connectionOptions);
            }

            return string.Empty;
        }

        private string BuildConnectionString(DatabaseConnectionOptions options)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = options.Server,
                InitialCatalog = options.DatabaseName,
                IntegratedSecurity = options.IntegratedSecurity,
                Password = options.Password,
                UserID = options.UserId,
                PersistSecurityInfo = options.PersistSecurityInfo,
                MultipleActiveResultSets = options.MultipleActiveResultSets,
                Encrypt = options.Encrypt,
                TrustServerCertificate = options.TrustServerCertificate,
                ConnectTimeout = options.ConnectTimeout
            };

            return builder.ConnectionString;
        }

        private string GetTenantName(UserContext UserContext)
        {
            if (UserContext == null)
                return Configuration["DatabaseSettings:Tenant:Name"];

            var claims = UserContext.Claims;
            var tenantName = Configuration["DatabaseSettings:Tenant:Name"];

            try
            {
                tenantName = claims.First(c => c.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception message from GetTenantName(): {ex.Message}");
                return tenantName;
            }

            return tenantName;
        }


        #region -------------------- DbSets -----------------------------
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        #endregion
    }

}
