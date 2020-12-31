using Microsoft.EntityFrameworkCore;
using Migrations.Core.Entities;
using Migrations.Core.ValueObjects;
using SharedKernel;

namespace Migrations.Data
{
    public class AppDbContext : DbContext
    {
        //const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        const string CONNECTION = "Server=localhost;Database=StockTracDomain;Trusted_Connection=True;";

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<SaleCode> SaleCodes { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<StatusRequirement> StatusRequirements { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(CONNECTION);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            SingularizeTableNames(modelBuilder);

            MapValueObjectsToOwningEntities(modelBuilder);

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().Property(e => e.Id).ValueGeneratedOnAdd();

            //modelBuilder.Entity<Person>()
            //            .HasMany(b => b.Phones)
            //            .WithOne();
            modelBuilder.Entity<Organization>()
                        .HasMany(b => b.Phones)
                        .WithOne();

            IgnoreColumns(modelBuilder);
        }

        private static void MapValueObjectsToOwningEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person")
                .OwnsOne(p => p.Name).Property(p => p.FirstName).HasColumnName("FirstName")
                .IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Person>().ToTable("Person")
                .OwnsOne(p => p.Name).Property(p => p.LastName).HasColumnName("LastName")
                .IsRequired().HasMaxLength(255); ;
            modelBuilder.Entity<Person>().ToTable("Person")
                .OwnsOne(p => p.Name).Property(p => p.MiddleName).HasColumnName("MiddleName");
            modelBuilder.Entity<Person>().ToTable("Person")
                .OwnsOne(p => p.DriversLicence).Property(p => p.Number).HasColumnName("DriversLicenseNumber")
                .IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Person>().ToTable("Person")
                .OwnsOne(p => p.DriversLicence).Property(p => p.State).HasColumnName("DriversLicenseState")
                .IsRequired().HasMaxLength(2);
            modelBuilder.Entity<Person>().ToTable("Person")
                 .OwnsOne(p => p.DriversLicence).Property(p => p.ValidRange).HasColumnName("DriversLicenseValidFromThru")
                 .IsRequired();
            modelBuilder.Entity<DriversLicence>()
                 .OwnsOne(p => p.ValidRange).Property(p => p.Start).HasColumnName("DriversLicenseIssued");
            modelBuilder.Entity<DriversLicence>()
                 .OwnsOne(p => p.ValidRange).Property(p => p.End).HasColumnName("DriversLicenseExpired");
        }

        private static void IgnoreColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Customer>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Employee>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Person>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Organization>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<ServiceRequest>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<StatusRequirement>().Ignore(p => p.TrackingState);
            //modelBuilder.Entity<Ticket>().Ignore(p => p.TrackingState);

            modelBuilder.Entity<Customer>().Ignore(p => p.Entity);
            modelBuilder.Entity<Customer>().Ignore(p => p.Address);
            modelBuilder.Entity<Customer>().Ignore(p => p.Phones);

            modelBuilder.Entity<Employee>().Ignore(p => p.Active);

        }

        private static void SingularizeTableNames(ModelBuilder modelBuilder)
        {
            // Prefer singular table names in SQL Server
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Customer>().ToTable("Customer");
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
        }
    }
}
