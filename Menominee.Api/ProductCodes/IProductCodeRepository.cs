using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.ProductCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.ProductCodes
{
    public interface IProductCodeRepository
    {
        void Add(ProductCode entity);
        void Delete(ProductCode entity);
        Task<ProductCode> GetEntityAsync(long id);
        Task<ProductCode> GetEntityAsync(long manufacturerId, string productCode);
        Task<ProductCodeToRead> GetAsync(long manufacturerId, string productCode);
        Task<ProductCodeToRead> GetAsync(long Id);
        Task SaveChangesAsync();
        IReadOnlyList<string> GetManufacturerCodes();
        Task<IReadOnlyList<ProductCode>> GetEntitiesAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetListAsync(long? manufacturerId, long? saleCodeId);
    }
}
