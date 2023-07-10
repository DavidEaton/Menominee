using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.SaleCodes
{
    public interface ISaleCodeDataService
    {
        Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodesAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllSaleCodeShopSuppliesAsync();
        Task<SaleCodeToRead> GetSaleCodeAsync(long id);
        Task<SaleCodeToRead> AddSaleCodeAsync(SaleCodeToWrite saleCode);
        Task UpdateSaleCodeAsync(SaleCodeToWrite saleCode, long id);
    }
}
