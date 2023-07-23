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
            foreach (var entity in entities)
            {
                if (entity.Id > 0)
                {
                    dbContext.Attach(entity);
                }
                else
                {
                    dbContext.Add(entity);
                }
            }

            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }

    }
}
