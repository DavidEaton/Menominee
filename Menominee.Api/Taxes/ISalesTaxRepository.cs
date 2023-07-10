using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Taxes
{
    public interface ISalesTaxRepository
    {
        Task AddSalesTaxAsync(SalesTax salesTax);
        Task<SalesTax> GetSalesTaxEntityAsync(long id);
        Task<SalesTaxToRead> GetSalesTaxAsync(long id);
        Task<IReadOnlyList<SalesTaxToReadInList>> GetSalesTaxListAsync();
        Task<IReadOnlyList<SalesTaxToRead>> GetSalesTaxesAsync();
        Task<SalesTax> UpdateSalesTaxAsync(SalesTax salesTax);
        Task DeleteSalesTaxAsync(long id);
        Task<bool> SalesTaxExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        Task<IReadOnlyList<SalesTax>> GetSalesTaxEntities();
    }
}
