using EntityApp.Domain.Classes;
using Microsoft.EntityFrameworkCore;

namespace EntityApp.Data
{
    public class AppDbContext : DbContext
    {
        const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracApiPoc;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connection);
        }
    }
}
