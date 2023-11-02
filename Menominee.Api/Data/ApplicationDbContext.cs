using CSharpFunctionalExtensions;
using Menominee.Api.Businesses;
using Menominee.Api.Company;
using Menominee.Api.Configurations;
using Menominee.Api.Configurations.CreditCards;
using Menominee.Api.Configurations.Inventory;
using Menominee.Api.Configurations.Payables;
using Menominee.Api.Configurations.RepairOrders;
using Menominee.Api.Configurations.Settings;
using Menominee.Api.Configurations.Taxes;
using Menominee.Api.Customers;
using Menominee.Api.Emails;
using Menominee.Api.Employees;
using Menominee.Api.Persons;
using Menominee.Api.Phones;
using Menominee.Api.SellingPriceNames;
using Menominee.Api.Users;
using Menominee.Api.Vehicles;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Entities.Settings;
using Menominee.Domain.Entities.Taxes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Linq;
using Entities = Menominee.Domain.Entities;

namespace Menominee.Api.Data
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
                options
                    .UseSqlServer(Connection)
                    .EnableSensitiveDataLogging(); // TODO: Conditionally set this option: 
                                                   // if (Environment.IsEnvironment("Testing"))

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
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());

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
            modelBuilder.ApplyConfiguration(new RepairOrderItemLaborConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderItemPartConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderLineItemConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderItemTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderPurchaseConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderSerialNumberConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderServiceConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderServiceTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderTaxConfiguration());
            modelBuilder.ApplyConfiguration(new RepairOrderWarrantyConfiguration());

            modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());

            modelBuilder.ApplyConfiguration(new SaleCodeConfiguration());

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
            modelBuilder.ApplyConfiguration(new InventoryItemWarrantyConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceItemConfiguration());

            modelBuilder.ApplyConfiguration(new ExciseFeeConfiguration());
            modelBuilder.ApplyConfiguration(new SalesTaxConfiguration());
            modelBuilder.ApplyConfiguration(new CreditCardConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());

            // Selling Price Names
            modelBuilder.ApplyConfiguration(new SellingPriceNameConfiguration());
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
            if (userContext is null)
                return Configuration["DatabaseSettings:Tenant:Name"];

            var claims = userContext?.Claims;
            var tenantName = Configuration["DatabaseSettings:Tenant:Name"];

            try
            {
                tenantName = claims?.First(claim => claim?.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception message from GetTenantName(): {ex.Message}");
                return "menominee-stage";
            }

            return tenantName;
        }

        #region -------------------- DbSets -----------------------------
        public DbSet<Entities.Company> Companies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorInvoice> VendorInvoices { get; set; }
        public DbSet<VendorInvoiceLineItem> VendorInvoiceLineItems { get; set; }
        public DbSet<VendorInvoicePayment> VendorInvoicePayments { get; set; }
        public DbSet<VendorInvoiceTax> VendorInvoiceTaxes { get; set; }
        public DbSet<VendorInvoicePaymentMethod> VendorInvoicePaymentMethods { get; set; }
        public DbSet<SaleCodeShopSupplies> SaleCodeShopSupplies { get; set; }

        public DbSet<RepairOrder> RepairOrders { get; set; }

        // ONLY CREATE DbSet<> FOR AGGREGATE ROOT 
        //public DbSet<RepairOrderService> RepairOrderServices { get; set; }
        public DbSet<RepairOrderItem> RepairOrderItems { get; set; }
        public DbSet<RepairOrderLineItem> RepairOrderLineItems { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<SaleCode> SaleCodes { get; set; }
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
        public DbSet<InventoryItemWarranty> InventoryItemWarranties { get; set; }
        public DbSet<MaintenanceItem> MaintenanceItems { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<ExciseFee> ExciseFees { get; set; }
        public DbSet<SalesTax> SalesTaxes { get; set; }
        public DbSet<ConfigurationSetting> Settings { get; set; }

        // Selling Price Names
        public DbSet<SellingPriceName> SellingPriceNames { get; set; }
        #endregion
    }

}
