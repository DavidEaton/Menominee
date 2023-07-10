using Menominee.Api.Data;
using Menominee.Tests.Helpers;
using Menominee.Common;
using System.Collections.Generic;

namespace Menominee.Tests.Integration.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ApplicationDbContext dbContext;

        public DataSeeder(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save<T>(List<T> entities) where T : Entity
        {
            dbContext.AddRange(entities);
            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }
    }
}
