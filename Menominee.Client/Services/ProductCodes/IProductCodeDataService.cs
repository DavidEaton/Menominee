﻿using Menominee.Shared.Models.ProductCodes;

namespace Menominee.Client.Services.ProductCodes
{
    public interface IProductCodeDataService
    {
        Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync(long mfrId);
        Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync(long mfrId, long saleCodeId);
        Task<ProductCodeToRead> GetProductCodeAsync(long id);
        Task<ProductCodeToRead> AddProductCodeAsync(ProductCodeToWrite productCode);
        Task UpdateProductCodeAsync(ProductCodeToWrite productCode, long id);
    }
}
