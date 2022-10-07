﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.ProductCodes
{
    public interface IProductCodeRepository
    {
        Task AddProductCodeAsync(ProductCode productCode);
        Task<ProductCode> GetProductCodeEntityAsync(string manufacturerCode, string productCode);
        Task<ProductCode> GetProductCodeEntityAsync(long id);
        Task<ProductCodeToRead> GetProductCodeAsync(string manufacturerCode, string productCode);
        Task<ProductCodeToRead> GetProductCodeAsync(long id);
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long manufacturerId);
        Task<ProductCode> UpdateProductCodeAsync(ProductCode productCode);
        Task DeleteProductCodeAsync(string manufacturerCode, string productCode);
        Task<bool> ProductCodeExistsAsync(string manufacturerCode, string code);
        Task<bool> ProductCodeExistsAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
