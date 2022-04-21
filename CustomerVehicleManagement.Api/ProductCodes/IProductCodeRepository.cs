using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.ProductCodes
{
    public interface IProductCodeRepository
    {
        Task AddProductCodeAsync(ProductCode productCode);
        Task<ProductCode> GetProductCodeEntityAsync(string manufacturerCode, string code);
        Task<ProductCodeToRead> GetProductCodeAsync(string manufacturerCode, string code);
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long mfrId);
        //Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long mfrId, long saleCodeId);
        //Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync(string manufacturerCode);
        Task<ProductCode> UpdateProductCodeAsync(ProductCode productCode);
        Task DeleteProductCodeAsync(string manufacturerCode, string code);
        Task<bool> ProductCodeExistsAsync(string manufacturerCode, string code);
        Task<bool> ProductCodeExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
