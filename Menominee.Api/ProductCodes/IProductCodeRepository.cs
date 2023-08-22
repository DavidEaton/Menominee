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
        Task<ProductCode> GetProductCodeEntityAsync(long id);
        Task<ProductCodeToRead> GetProductCodeAsync(long manufacturerId, string productCode);
        Task<ProductCodeToRead> GetProductCodeAsync(long id);
        Task<IReadOnlyList<ProductCodeToRead>> GetProductCodesAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long? manufacturerId, long? saleCodeId);
        Task DeleteProductCodeAsync(long id);
        Task<bool> ProductCodeExistsAsync(long id);
        Task SaveChangesAsync();
        IReadOnlyList<string> GetManufacturerCodes();
    }
}
