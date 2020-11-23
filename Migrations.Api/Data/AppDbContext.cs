using Microsoft.EntityFrameworkCore;
using Migrations.Core.Entities;

namespace Migrations.Api.Data
{
    public class AppDbContext : DbContext
    {
        const string CONNECTION = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        #region -------------------- DbSets -----------------------------

        //public DbSet<Address> Addresses { get; set; } // Can't create a DbSet<T> of an owned type (by design).
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DriversLicence> DriversLicences { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<SaleCode> SaleCodes { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<StatusRequirement> StatusRequirements { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(CONNECTION);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<DriversLicence>().OwnsOne(p => p.ValidFromThru);
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<DriversLicence>().ToTable("DriversLicence");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<EmployeeRole>().ToTable("EmployeeRole");
            modelBuilder.Entity<Organization>().ToTable("Organization");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<SaleCode>().ToTable("SaleCode");
            modelBuilder.Entity<ServiceRequest>().ToTable("ServiceRequest");
            modelBuilder.Entity<StatusRequirement>().ToTable("StatusRequirement");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");

            modelBuilder.Entity<Person>()
                        .HasMany(b => b.Phones)
                        .WithOne();
            modelBuilder.Entity<Organization>()
                        .HasMany(b => b.Phones)
                        .WithOne();
        }

    }
}
