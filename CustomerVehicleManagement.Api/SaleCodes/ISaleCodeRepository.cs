using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.SaleCodes
{
    public interface ISaleCodeRepository
    {
        Task AddSaleCodeAsync(SaleCode saleCode);
        Task<SaleCode> GetSaleCodeEntityAsync(string code);
        Task<SaleCode> GetSaleCodeEntityAsync(long id);
        Task<SaleCodeToRead> GetSaleCodeAsync(string code);
        Task<SaleCodeToRead> GetSaleCodeAsync(long id);
        Task<IReadOnlyList<SaleCodeToReadInList>> GetSaleCodeListAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetSaleCodeShopSuppliesListAsync();
        Task<IReadOnlyList<SaleCodeShopSuppliesToRead>> GetSaleCodeShopSupplies(long Id);
        Task DeleteSaleCodeAsync(long id);
        Task<bool> SaleCodeExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        Task<IReadOnlyList<SaleCode>> GetSaleCodeEntitiesAsync(List<long> ids);
    }
}
