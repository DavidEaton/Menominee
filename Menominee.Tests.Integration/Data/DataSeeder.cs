using Menominee.Api.Data;
using Menominee.Domain.BaseClasses;
using Menominee.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
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
            entities.ForEach(entity => AddOrAttach(entity, dbContext));
            DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
        }

        private static void AddOrAttach<T>(T entity, DbContext dbContext) where T : Entity
        {
            var dbSet = dbContext.Set<T>();
            if (entity.Id > 0)
                dbSet.Attach(entity);
            else
                dbSet.Add(entity);
        }
    }
}
