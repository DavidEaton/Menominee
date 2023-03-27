using CustomerVehicleManagement.Api.Configurations;
using CustomerVehicleManagement.Api.Configurations.CreditCards;
using CustomerVehicleManagement.Api.Configurations.Inventory;
using CustomerVehicleManagement.Api.Configurations.Payables;
using CustomerVehicleManagement.Api.Configurations.RepairOrders;
using CustomerVehicleManagement.Api.Configurations.Taxes;
using CustomerVehicleManagement.Api.Users;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Domain.Entities.Taxes;
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
        private string Connection = string.Empty;
        readonly ILogger<ApplicationDbContext> Logger;
        public ApplicationDbContext() { }

        public ApplicationDbContext(string connection)
        {
            // Database & integration tests provide connection string
            Connection = connection;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { } // tests pass in options

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
            // TODO: remove pollution of production code with test concerns:
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

            // Payables
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceLineItemConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoicePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoicePaymentMethodConfiguration());
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

            // Manufacturers
            modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());

            // Sale Codes
            modelBuilder.ApplyConfiguration(new SaleCodeConfiguration());

            // Product Codes
            modelBuilder.ApplyConfiguration(new ProductCodeConfiguration());

            // Inventory
            modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemPartConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemLaborConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemTireConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemPackageConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemPackageItemConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemPackagePlaceholderConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemInspectionConfiguration());
            // Coming soon...
            //modelBuilder.ApplyConfiguration(new InventoryItemDonationConfiguration());
            //modelBuilder.ApplyConfiguration(new InventoryItemGiftCertificateConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemWarrantyConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceItemConfiguration());

            // Taxes/Fees
            modelBuilder.ApplyConfiguration(new ExciseFeeConfiguration());
            modelBuilder.ApplyConfiguration(new SalesTaxConfiguration());

            // Credit Cards
            modelBuilder.ApplyConfiguration(new CreditCardConfiguration());
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

        private string GetTenantName(UserContext userContext)
        {
            if (userContext == null)
                return Configuration["DatabaseSettings:Tenant:Name"];

            var claims = userContext.Claims;
            var tenantName = Configuration["DatabaseSettings:Tenant:Name"];

            try
            {
                tenantName = claims.First(claim => claim.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception message from GetTenantName(): {ex.Message}");
                tenantName = "menominee-stage";
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
        public DbSet<VendorInvoiceLineItem> VendorInvoiceLineItems { get; set; }
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

        // Manufacturers
        public DbSet<Manufacturer> Manufacturers { get; set; }

        // Sale Codes
        public DbSet<SaleCode> SaleCodes { get; set; }

        // Product Codes
        public DbSet<ProductCode> ProductCodes { get; set; }

        // Inventory
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemPart> InventoryItemParts { get; set; }
        public DbSet<InventoryItemLabor> InventoryItemLabor { get; set; }
        public DbSet<InventoryItemTire> InventoryItemTires { get; set; }
        public DbSet<InventoryItemPackage> InventoryItemPackages { get; set; }
        public DbSet<InventoryItemPackageItem> InventoryItemPackageItems { get; set; }
        public DbSet<InventoryItemPackagePlaceholder> InventoryItemPackagePlaceholders { get; set; }
        public DbSet<InventoryItemInspection> InventoryItemInspections { get; set; }
        //public DbSet<InventoryItemDonation> InventoryItemDonations { get; set; }
        //public DbSet<InventoryItemGiftCertificate> InventoryItemGiftCertificates { get; set; }
        public DbSet<InventoryItemWarranty> InventoryItemWarranties { get; set; }
        public DbSet<MaintenanceItem> MaintenanceItems { get; set; }

        // Credit Cards
        public DbSet<CreditCard> CreditCards { get; set; }

        // Taxes/Fees
        public DbSet<ExciseFee> ExciseFees { get; set; }
        public DbSet<SalesTax> SalesTaxes { get; set; }
        #endregion
    }

}
