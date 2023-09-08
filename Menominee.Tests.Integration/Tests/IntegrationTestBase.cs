using Bogus;
using Menominee.Api.Data;
using Menominee.Tests.Integration.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Menominee.Tests.Integration.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>, IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly IDataSeeder dataSeeder;
        protected readonly ApplicationDbContext dbContext;
        protected readonly Faker faker;

        protected IntegrationTestBase(IntegrationTestWebApplicationFactory factory)
        {
            httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost/api/")
            });

            faker = new Faker();
            dataSeeder = factory.Services.GetRequiredService<IDataSeeder>();
            dbContext = factory.Services.GetRequiredService<ApplicationDbContext>();

            EnsureDatabaseMigration();
            SeedData();
        }

        private void EnsureDatabaseMigration()
        {
            var migrationSuccess = false;
            var maxRetries = 1;
            var retryCount = 0;

            while (!migrationSuccess && retryCount < maxRetries)
            {
                try
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.Migrate();
                    migrationSuccess = true;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                }
            }

            if (!migrationSuccess)
            {
                var errorMessage = $"Database migration failed after {maxRetries} retries.";
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public abstract void SeedData();
        public abstract void Dispose();
    }

}
