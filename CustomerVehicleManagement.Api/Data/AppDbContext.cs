using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerVehicleManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        //const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        const string CONNECTION = "Server=localhost;Database=Menominee;Trusted_Connection=True;";

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(CONNECTION);
        }
    }
}
