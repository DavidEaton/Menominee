using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> AllAsync();
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindByKeyAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
    }
}