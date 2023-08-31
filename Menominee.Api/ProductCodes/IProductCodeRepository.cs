using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.ProductCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.ProductCodes
{
    public interface IProductCodeRepository
    {
        void AddProductCode(ProductCode productCode);
        Task<ProductCode> GetProductCodeEntityAsync(long manufacturerId, string productCode);
        Task<ProductCodeToRead> GetProductCodeAsync(long manufacturerId, string productCode);
        Task<ProductCodeToRead> GetProductCodeAsync(long Id);
        Task DeleteProductCode(long id);
        Task SaveChangesAsync();
        IReadOnlyList<string> GetManufacturerCodes();
        Task<IReadOnlyList<ProductCode>> GetProductCodeEntitiesAsync();
        Task<ProductCode> GetProductCodeEntityAsync(long id);
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long? manufacturerId, long? saleCodeId);
        Task<bool> ProductCodeExists(long id);
    }
}
