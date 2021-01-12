using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity FindByKey(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
    }
}