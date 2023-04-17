using CustomerVehicleManagement.Api.Data;
using Menominee.Common;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Integration.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ApplicationDbContext dbContext;

        public DataSeeder(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedData<T>(List<T> entities) where T : Entity
        {
            dbContext.AddRange(entities);
            dbContext.SaveChanges();
        }
    }
}
