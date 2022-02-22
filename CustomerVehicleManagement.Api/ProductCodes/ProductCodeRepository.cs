using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.ProductCodes
{
    public class ProductCodeRepository : IProductCodeRepository
    {
        private readonly ApplicationDbContext context;

        public ProductCodeRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddProductCodeAsync(ProductCode productCode)
        {
            if (productCode != null)
                await context.AddAsync(productCode);
        }

        public async Task DeleteProductCodeAsync(string manufacturerCode, string code)
        {
            var pcFromContext = await context.ProductCodes.FindAsync(manufacturerCode, code);
            if (pcFromContext != null)
                context.Remove(pcFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(string manufacturerCode, string code)
        {
            var pcFromContext = await context.ProductCodes
                .FirstOrDefaultAsync(pc => (pc.Manufacturer.Code == manufacturerCode && pc.Code == code));

            return ProductCodeToRead.ConvertToDto(pcFromContext);
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(string manufacturerCode, string code)
        {
            var pcFromContext = await context.ProductCodes
                .FirstOrDefaultAsync(pc => (pc.Manufacturer.Code == manufacturerCode && pc.Code == code));

            return pcFromContext;
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync()
        {
            IReadOnlyList<ProductCode> pcs = await context.ProductCodes.ToListAsync();

            return pcs.
                Select(pc => ProductCodeToReadInList.ConvertToDto(pc))
                .ToList();
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync(long mfrId, long saleCodeId)
        {
            IReadOnlyList<ProductCode> pcs = await context.ProductCodes
                .Where(pc => (pc.Manufacturer.Id == mfrId && pc.SaleCode.Id == saleCodeId))
                .ToListAsync();

            return pcs.
                Select(pc => ProductCodeToReadInList.ConvertToDto(pc))
                .ToList();
        }

        //public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync(string manufacturerCode)
        //{
        //    IReadOnlyList<ProductCode> pcs = await context.ProductCodes.Where(pc => pc.Manufacturer.Code == manufacturerCode).ToListAsync();

        //    return pcs.
        //        Select(pc => ProductCodeToReadInList.ConvertToDto(pc))
        //        .ToList();
        //}

        public async Task<bool> ProductCodeExistsAsync(string manufacturerCode, string code)
        {
            return await context.ProductCodes.AnyAsync(pc => (pc.Manufacturer.Code == manufacturerCode && pc.Code == code));
        }

        public async Task<bool> SaveChangesAsync()
        { 
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateProductCodeAsync(ProductCode productCode)
        {
            // No code in this implementation
        }
    }
}
