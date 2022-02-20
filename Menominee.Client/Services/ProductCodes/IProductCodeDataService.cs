using CustomerVehicleManagement.Shared.Models.ProductCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.ProductCodes
{
    public interface IProductCodeDataService
    {
        Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodes();
        Task<ProductCodeToRead> GetProductCode(long id);
        Task<ProductCodeToRead> AddManufacturer(ProductCodeToWrite productCode);
        Task UpdateProductCode(ProductCodeToWrite productCode, long id);
    }
}
