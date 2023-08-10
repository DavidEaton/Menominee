using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Businesses
{
    public interface IBusinessRepository
    {
        Task AddBusinessAsync(Business entity);
        Task<IReadOnlyList<BusinessToRead>> GetBusinessesAsync();
        Task<BusinessToRead> GetBusinessAsync(long id);
        Task<IReadOnlyList<BusinessToReadInList>> GetBusinessesListAsync();
        void DeleteBusiness(Business entity);
        Task<bool> BusinessExistsAsync(long id);
        Task SaveChangesAsync();
        Task<Business> GetBusinessEntityAsync(long id);
    }
}