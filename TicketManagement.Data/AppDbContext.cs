using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.Model;
using Vehicle = CustomerVehicleManagement.Core.Model.Vehicle;

namespace TicketManagement.Data
{
    public class AppDbContext : DbContext
    {
        const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTrac;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<SaleCode> ServiceTypes { get; set; }
        public DbSet<StatusRequirement> StatusRequirements { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connection);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    // Preference for singular table names in SQL Server
        //    modelBuilder.Entity<Customer>().ToTable("Customer");
        //    modelBuilder.Entity<ServiceRequest>().ToTable("ServiceRequest");
        //    modelBuilder.Entity<ServiceType>().ToTable("ServiceType");
        //    modelBuilder.Entity<StatusRequirement>().ToTable("StatusRequirement");
        //    modelBuilder.Entity<Ticket>().ToTable("Ticket");
        //    modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus");
        //    modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        //}
    }
}
