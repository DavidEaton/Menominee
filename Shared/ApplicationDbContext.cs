using MenomineePlayWASM.Shared.DbConfigurations.Inventory;
using MenomineePlayWASM.Shared.DbConfigurations.Payables;
using MenomineePlayWASM.Shared.DbConfigurations.RepairOrders;
using MenomineePlayWASM.Shared.Entities;
using MenomineePlayWASM.Shared.Entities.Customers;
using MenomineePlayWASM.Shared.Entities.Inventory;
using MenomineePlayWASM.Shared.Entities.Payables.CreditReturns;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;

namespace MenomineePlayWASM.Shared
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<xxx>().HasKey(x => new { x.yyy, x.zzz });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Ignore<Entity>();

            // Inventory
            modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
            modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());

            // Repair Orders
            modelBuilder.ApplyConfiguration(new RepairOrderConfiguration());

            // Payables
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceItemConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoicePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new VendorInvoiceTaxConfiguration());
        }

        // Payables
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorInvoice> VendorInvoices { get; set; }
        public DbSet<VendorInvoiceItem> VendorInvoiceItems { get; set; }
        public DbSet<VendorInvoicePayment> VendorInvoicePayments { get; set; }
        public DbSet<VendorInvoiceTax> VendorInvoiceTaxes { get; set; }
        public DbSet<VendorInvoicePaymentMethod> VendorInvoicePaymentMethods { get; set; }
        public DbSet<CreditReturn> CreditReturns { get; set; }
        public DbSet<CreditReturnItem> CreditReturnItems { get; set; }

        // Inventory
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        // Customers
        public DbSet<Customer> Customers { get; set; }

        // Repair Orders
        public DbSet<RepairOrder> RepairOrders { get; set; }
    }
}
