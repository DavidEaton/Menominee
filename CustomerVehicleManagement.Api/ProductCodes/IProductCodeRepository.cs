using CustomerVehicleManagement.Domain.Entities;
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
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync();
        Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync(string manufacturerCode);
        void UpdateProductCodeAsync(ProductCode productCode);
        Task DeleteProductCodeAsync(string manufacturerCode, string code);
        Task<bool> ProductCodeExistsAsync(string manufacturerCode, string code);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
