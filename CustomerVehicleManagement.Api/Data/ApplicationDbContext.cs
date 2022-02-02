using CustomerVehicleManagement.Api.Configurations;
using CustomerVehicleManagement.Api.Configurations.RepairOrders;
using CustomerVehicleManagement.Api.Users;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CustomerVehicleManagement.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment Environment;
        private readonly UserContext UserContext;
        readonly ILogger<ApplicationDbContext> Logger;
        private string Connection = string.Empty;

        public ApplicationDbContext() { }

        public ApplicationDbContext(string connection)
        {
            Connection = connection;  // Database integration tests pass in connection
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { } // Unit tests pass in options

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            UserContext userContext,
            ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            Environment = environment;
            Configuration = configuration;
            UserContext = userContext;
            Logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (UserContext != null) // Unit tests do not yet inject UserContext
                Connection = GetTenantConnection();

            if (!options.IsConfigured) // Unit tests will configure context with test provider
                options.UseSqlServer(Connection);

            base.OnConfiguring(options);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Ignore<Entity>();

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceConfiguration());

            // Payables
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceItemConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoicePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceTaxConfiguration());

            // Repair Orders
            modelBuilder.ApplyConfiguration(new RepairOrderConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderItemTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderPurchaseConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderSerialNumberConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderServiceConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderServiceTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderTechConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderWarrantyConfiguration());

        }

        private string GetTenantConnection()
        {
            string tenantName = GetTenantName(UserContext);

            if (!string.IsNullOrWhiteSpace(tenantName))
            {
                var connectionOptions = new DatabaseConnectionOptions
                {
                    DatabaseName = tenantName,
                    Server = Configuration["DatabaseSettings:Server:Name"],
                    IntegratedSecurity = Environment.EnvironmentName == "Development",
                    Password = Configuration["DatabaseSettings:Server:Password"],
                    UserId = Configuration["DatabaseSettings:Server:UserName"],
                    TrustServerCertificate = Environment.EnvironmentName == "Development"
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
                tenantName = claims.First(claim => claim.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception message from GetTenantName(): {ex.Message}");
                return tenantName;
            }

            return tenantName;
        }


        #region -------------------- DbSets -----------------------------
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorInvoice> VendorInvoices { get; set; }
        public DbSet<VendorInvoiceItem> VendorInvoiceItems { get; set; }
        public DbSet<VendorInvoicePayment> VendorInvoicePayments { get; set; }
        public DbSet<VendorInvoiceTax> VendorInvoiceTaxes { get; set; }
        public DbSet<VendorInvoicePaymentMethod> VendorInvoicePaymentMethods { get; set; }

        // Repair Orders
        public DbSet<RepairOrder> RepairOrders { get; set; }
        public DbSet<RepairOrderService> RepairOrderServices { get; set; }
        public DbSet<RepairOrderItem> RepairOrderItems { get; set; }
        public DbSet<RepairOrderItemTax> RepairOrderItemTaxes { get; set; }
        public DbSet<RepairOrderSerialNumber> RepairOrderSerialNumbers { get; set; }
        public DbSet<RepairOrderPurchase> RepairOrderPurchases { get; set; }
        public DbSet<RepairOrderWarranty> RepairOrderWarranties { get; set; }
        public DbSet<RepairOrderServiceTax> RepairOrderServiceTaxes { get; set; }
        public DbSet<RepairOrderTech> RepairOrderTechs { get; set; }
        public DbSet<RepairOrderTax> RepairOrderTaxes { get; set; }
        public DbSet<RepairOrderPayment> RepairOrderPayments { get; set; }

        #endregion
    }

}
