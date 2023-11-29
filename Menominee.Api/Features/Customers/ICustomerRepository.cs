using Menominee.Domain.Entities;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Customers
{
    public interface ICustomerRepository
    {
        void Add(Customer entity);
        void Delete(Customer entity);
        Task<IReadOnlyList<CustomerToRead?>> GetAllAsync();
        Task<PagedList<CustomerToRead?>> GetByCodeAsync(string code, Pagination pagination);
        Task<IReadOnlyList<CustomerToReadInList?>> GetListAsync();
        Task<CustomerToRead?> GetAsync(long id);
        Task SaveChangesAsync();
        Task<Customer?> GetEntityAsync(long id);
    }
}
