using CustomerVehicleManagement.Data.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace CustomerVehicleManagement.Data
{
    /// <summary>
    /// This DbContext is used only to facilitate databaae migrations
    /// </summary>
    public class DataDbContext : DbContext
    {
        //const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private const string Connection = "Server=localhost;Database=Menominee;Trusted_Connection=True;";

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Connection);
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
        }
    }
}
