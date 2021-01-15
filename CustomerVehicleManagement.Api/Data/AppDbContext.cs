using CustomerVehicleManagement.Data.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace CustomerVehicleManagement.Api.Data
{
    /// <summary>
    /// This DbContext is used by the api, injected into the services Inversion of Control (IoC) container during Startup
    /// </summary>
    public class AppDbContext : DbContext
    {
        //const string connection = "Server=tcp:janco.database.windows.net,1433;Initial Catalog=StockTracDomain;Persist Security Info=False;User ID=jancoAdmin;Password=sd5hF4Z4zcpoc!842;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //const string CONNECTION = "Server=localhost;Database=Menominee;Trusted_Connection=True;";

        public IHostEnvironment environment;
        public IConfiguration configuration;
        private readonly ILogger<AppDbContext> logger;
        //private readonly UserContext userContext;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            //UserContext userContext,
            IHostEnvironment environment,
            IConfiguration configuration,
            ILogger<AppDbContext> logger)
            : base(options)
        {
            //this.userContext = userContext;
            this.environment = environment;
            this.configuration = configuration;
            this.logger = logger;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Phone> Phones { get; set; }

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
