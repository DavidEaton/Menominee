using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.ProductCodes;

namespace Menominee.Client.Services.ProductCodes
{
    public interface IProductCodeDataService
    {
        Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetAllAsync();
        Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetByManufacturerAsync(long manufacturerId);
        Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetByManufacturerAndSaleCodeAsync(long manufacturerId, long saleCodeId);
        Task<Result<ProductCodeToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(ProductCodeToWrite productCode);
        Task<Result> UpdateAsync(ProductCodeToWrite productCode);
    }
}
