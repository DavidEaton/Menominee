using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking()
                              .Where(predicate)
                              .ToListAsync();
        }

        public async Task<TEntity> FindByKeyAsync(int id)
        {
            return await dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
