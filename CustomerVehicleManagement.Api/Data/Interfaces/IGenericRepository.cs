using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> AllAsync();
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindByKeyAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
    }
}