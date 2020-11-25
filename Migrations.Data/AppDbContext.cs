using Microsoft.EntityFrameworkCore;
using Migrations.Core.Entities;
using Migrations.Core.ValueObjects;

namespace Migrations.Data
{
    public class AppDbContext : DbContext
    {
        //const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        const string connection = "Server=localhost;Database=StockTracDomain;Trusted_Connection=True;";

        //public DbSet<Address> Addresses { get; set; } // You can't create a DbSet<T> of an owned type (by design).
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<DriversLicence> DriversLicences { get; set; } // You can't create a DbSet<T> of an owned type (by design).
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<SaleCode> SaleCodes { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<StatusRequirement> StatusRequirements { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Preference for singular table names in SQL Server
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            //modelBuilder.Entity<DriversLicence>().ToTable("DriversLicence");
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

            modelBuilder.Entity<Person>().ToTable("Person")
                 .OwnsOne(p => p.DriversLicence)
                 .Property(p => p.Number).HasColumnName("Number");
            modelBuilder.Entity<Person>().ToTable("Person")
                 .OwnsOne(p => p.DriversLicence)
                 .Property(p => p.State).HasColumnName("State");




            modelBuilder.Entity<Person>()
                        .HasMany(b => b.Phones)
                        .WithOne();
            modelBuilder.Entity<Organization>()
                        .HasMany(b => b.Phones)
                        .WithOne();

            modelBuilder.Entity<Customer>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Employee>().Ignore(p => p.TrackingState);
            modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);
            modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);
            modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);
            modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);
            modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);

            //modelBuilder.Entity<PersonAddress>().ToTable("PersonAddress");
            //modelBuilder.Entity<OrganizationAddress>().ToTable("OrganizationAddress");

            // Address and Phone as ValueObject
            //modelBuilder.Entity<Organization>().ToTable("Organization")
            //                 .OwnsOne(o => o.Address);
            //modelBuilder.Entity<Person>().ToTable("Person")
            //                 .OwnsOne(p => p.Address);
            //modelBuilder.Entity<Organization>().OwnsOne(o => o.Phones);
            //modelBuilder.Entity<Person>().OwnsOne(o => o.Phones);

            //modelBuilder.Entity<Address>().Property(p => p.AddressLine).HasColumnName("AddressLine");
            //modelBuilder.Entity<Address>().Property(p => p.City).HasColumnName("City");
            //modelBuilder.Entity<Address>().Property(p => p.State).HasColumnName("State");
            //modelBuilder.Entity<Address>().Property(p => p.PostalCode).HasColumnName("PostalCode");
            //modelBuilder.Entity<Address>().Property(p => p.CountryCode).HasColumnName("CountryCode");

        }
    }
}
