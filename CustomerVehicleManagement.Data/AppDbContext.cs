using CustomerVehicleManagement.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomerVehicleManagement.Data
{
    public class AppDbContext : DbContext
    {
        const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracApiPoc;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<PersonAddress> PersonAddresses { get; set; }
        public DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DriversLicence>().OwnsOne(p => p.ValidFromThru);
            // Preference for singular table names in SQL Server
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Organization>().ToTable("Organization");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<PersonAddress>().ToTable("PersonAddress");
            modelBuilder.Entity<OrganizationAddress>().ToTable("OrganizationAddress");

            //modelBuilder.Entity<PersonAddress>()
            //            .HasKey(pa => new { pa.PersonId, pa.AddressId });
            //modelBuilder.Entity<PersonAddress>()
            //            .HasOne(pa => pa.Person)
            //            .WithMany(a => a.PersonAddresses)
            //            .HasForeignKey(pa => pa.PersonId);
            //modelBuilder.Entity<PersonAddress>()
            //            .HasOne(bc => bc.Address)
            //            .WithMany(c => c.PersonAddresses)
            //            .HasForeignKey(bc => bc.AddressId);
        }
    }
}
