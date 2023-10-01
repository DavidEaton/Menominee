using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Taxes
{
    public interface ISalesTaxRepository
    {
        void Add(SalesTax entity);
        void Delete(SalesTax entity);
        Task<SalesTax> GetEntityAsync(long id);
        Task<SalesTaxToRead> GetAsync(long id);
        Task<IReadOnlyList<SalesTaxToReadInList>> GetListAsync();
        Task<IReadOnlyList<SalesTaxToRead>> GetAllAsync();
        Task SaveChangesAsync();
        Task<IReadOnlyList<SalesTax>> GetEntitiesAsync();
    }
}
