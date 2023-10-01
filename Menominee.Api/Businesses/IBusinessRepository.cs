using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Businesses
{
    public interface IBusinessRepository
    {
        void Add(Business entity);
        void Delete(Business entity);
        Task<BusinessToRead> GetAsync(long id);
        Task<IReadOnlyList<BusinessToRead>> GetAllAsync();
        Task<IReadOnlyList<BusinessToReadInList>> GetListAsync();
        Task<Business> GetEntityAsync(long id);
        Task SaveChangesAsync();
    }
}