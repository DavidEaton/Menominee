using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Utilities;
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
            Guard.ForNull(productCode, "productCode");


            if (await ProductCodeExistsAsync(productCode.Id))
                throw new Exception("Inventory Item already exists");

            if (productCode != null)
                await context.AddAsync(productCode);
        }

        public async Task DeleteProductCodeAsync(string manufacturerCode, string code)
        {
            var pcFromContext = await context.ProductCodes.FindAsync(manufacturerCode, code); ;

            Guard.ForNull(pcFromContext, "pcFromContext");

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
                                             .Include(pc => pc.Manufacturer)
                                             .Include(pc => pc.SaleCode)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(pc => (pc.Manufacturer.Code == manufacturerCode && pc.Code == code));
            
            Guard.ForNull(pcFromContext, "pcFromContext");


            return ProductCodeToRead.ConvertToDto(pcFromContext);
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(long id)
        {
            var pcFromContext = await context.ProductCodes
                                             .Include(pc => pc.Manufacturer)
                                             .Include(pc => pc.SaleCode)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(pc => pc.Id == id);

            Guard.ForNull(pcFromContext, "pcFromContext");


            return ProductCodeToRead.ConvertToDto(pcFromContext);
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(string manufacturerCode, string code)
        {
            return await context.ProductCodes
                                .Include(pc => pc.Manufacturer)
                                .Include(pc => pc.SaleCode)
                                .FirstOrDefaultAsync(pc => (pc.Manufacturer.Code == manufacturerCode && pc.Code == code));
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync()
        {
            var pcs = await context.ProductCodes
                                   .Include(pc => pc.Manufacturer)
                                   .Include(pc => pc.SaleCode)
                                   .AsNoTracking()
                                   .ToArrayAsync();

            return pcs.Select(pc => ProductCodeToReadInList.ConvertToDto(pc)).ToList();
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long mfrId)
        {
            var pcs = await context.ProductCodes
                                   .Include(pc => pc.Manufacturer)
                                   .Include(pc => pc.SaleCode)
                                   .Where(pc => pc.Manufacturer.Id == mfrId)
                                   .AsNoTracking()
                                   .ToArrayAsync();

            return pcs.Select(pc => ProductCodeToReadInList.ConvertToDto(pc)).ToList();
        }

        //public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long mfrId, long saleCodeId)
        //{
        //    var pcs = await context.ProductCodes
        //                           .Include(pc => pc.Manufacturer)
        //                           .Include(pc => pc.SaleCode)
        //                           .Where(pc => (pc.Manufacturer.Id == mfrId && pc.SaleCode.Id == saleCodeId))
        //                           .AsNoTracking()
        //                           .ToArrayAsync();

        //    return pcs.Select(pc => ProductCodeToReadInList.ConvertToDto(pc)).ToList();
        //}

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
        
        public async Task<bool> ProductCodeExistsAsync(long id)
        {
            return await context.ProductCodes.AnyAsync(pc => (pc.Id == id));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<ProductCode> UpdateProductCodeAsync(ProductCode productCode)
        {
            Guard.ForNull(productCode, "productCode");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(productCode).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductCodeExistsAsync(productCode.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
