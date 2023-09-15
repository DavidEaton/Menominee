using Menominee.Api.Data;
using Menominee.Tests.Integration.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
    {
        protected readonly HttpClient HttpClient;
        protected readonly IDataSeeder DataSeeder;
        protected readonly ApplicationDbContext DbContext;
        protected Respawner Respawner;

        public IntegrationTestBase(IntegrationTestWebApplicationFactory factory)
        {
            HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost/api/")
            });

            DataSeeder = factory.Services.GetRequiredService<IDataSeeder>();
            DbContext = factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        public async Task InitializeAsync()
        {
            var databaseExists = (DbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
            if (!databaseExists)
                DbContext.Database.Migrate();

            var connection = DbContext.Database.GetDbConnection().ConnectionString;

            Respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                "__EFMigrationsHistory",
                "Manufacturer",
                }
            });

            SeedData();
        }

        public abstract void SeedData();
        public async Task DisposeAsync()
        {
            await Respawner.ResetAsync(DbContext.Database.GetDbConnection().ConnectionString);
        }
    }
}
