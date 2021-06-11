using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Data.Configurations;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
using SharedKernel.Enums;
using System;

namespace CustomerVehicleManagement.Api
{
    /// <summary>
    /// This DbContext is used by the api, injected into the services Inversion of Control (IoC) container during Startup
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly bool useConsoleLogger;
        private readonly string connection;

        protected AppDbContext() { }
        public AppDbContext(string connection, bool useConsoleLogger)
        {
            this.connection = connection;
            this.useConsoleLogger = useConsoleLogger;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // Unit tests will configure context with test provider
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                      .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                      .AddConsole();
                });

                optionsBuilder.UseSqlServer(connection)
                              .UseLazyLoadingProxies(false);

                if (useConsoleLogger)
                {
                    optionsBuilder
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging();
                }
            }
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

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
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
        }
    }
}
