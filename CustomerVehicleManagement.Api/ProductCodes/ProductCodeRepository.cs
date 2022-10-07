﻿using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
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
            if (productCode is not null)
            {
                if (await ProductCodeExistsAsync(productCode.Id))
                    throw new Exception("Inventory Item already exists");

                if (productCode is not null)
                    await context.AddAsync(productCode);
            }
        }

        public async Task DeleteProductCodeAsync(string manufacturerCode, string productCode)
        {
            var pcFromContext = await context.ProductCodes.FindAsync(manufacturerCode, productCode); ;

            if (pcFromContext is not null)
                context.Remove(pcFromContext);
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(string manufacturerCode, string code)
        {
            var pcFromContext = await context.ProductCodes
                                             .Include(productCode => productCode.Manufacturer)
                                             .Include(productCode => productCode.SaleCode)
                                             .AsSplitQuery()
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(productCode =>
                                                productCode.Manufacturer.Code == manufacturerCode
                                                &&
                                                productCode.Code == code);

            return pcFromContext is not null
                ? ProductCodeHelper.ConvertEntityToReadDto(pcFromContext)
                : null;
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(long id)
        {
            var pcFromContext = await context.ProductCodes
                                             .Include(pc => pc.Manufacturer)
                                             .Include(pc => pc.SaleCode)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(pc => pc.Id == id);

            return pcFromContext is not null
                ? ProductCodeHelper.ConvertEntityToReadDto(pcFromContext)
                : null;
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(string manufacturerCode, string code)
        {
            return await context.ProductCodes
                                .Include(productCode => productCode.Manufacturer)
                                .Include(productCode => productCode.SaleCode)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(productCode =>
                                    productCode.Manufacturer.Code == manufacturerCode
                                    &&
                                    productCode.Code == code);
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(long id)
        {
            return await context.ProductCodes
                                             .Include(productCode => productCode.Manufacturer)
                                             .Include(productCode => productCode.SaleCode)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(productCode => productCode.Id == id);
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync()
        {
            var pcs = await context.ProductCodes
                                   .Include(productCode => productCode.Manufacturer)
                                   .Include(productCode => productCode.SaleCode)
                                   .AsSplitQuery()
                                   .AsNoTracking()
                                   .ToArrayAsync();

            return pcs.
                Select(productCode => ProductCodeHelper.ConvertEntityToReadInListDto(productCode))
                .ToList();
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long manufacturerId)
        {
            var pcs = await context.ProductCodes
                                   .Include(productCode => productCode.Manufacturer)
                                   .Include(productCode => productCode.SaleCode)
                                   .Where(productCode => productCode.Manufacturer.Id == manufacturerId)
                                   .AsSplitQuery()
                                   .AsNoTracking()
                                   .ToArrayAsync();

            return pcs.
                Select(productCode => ProductCodeHelper.ConvertEntityToReadInListDto(productCode))
                .ToList();
        }

        //public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long mfrId, long saleCodeId)
        //{
        //    var pcs = await context.ProductCodes
        //                           .Include(productCode => productCode.Manufacturer)
        //                           .Include(productCode => productCode.SaleCode)
        //                           .Where(productCode => (productCode.Manufacturer.Id == mfrId && productCode.SaleCode.Id == saleCodeId))
        //                           .AsNoTracking()
        //                           .ToArrayAsync();

        //    return pcs.Select(productCode => ProductCodeToReadInList.ConvertToDto(productCode)).ToList();
        //}

        //public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodeListAsync(string manufacturerCode)
        //{
        //    IReadOnlyList<ProductCode> pcs = await context.ProductCodes.Where(productCode => productCode.Manufacturer.Code == manufacturerCode).ToListAsync();

        //    return pcs.
        //        Select(productCode => ProductCodeToReadInList.ConvertToDto(productCode))
        //        .ToList();
        //}

        public async Task<bool> ProductCodeExistsAsync(string manufacturerCode, string code)
        {
            return await context.ProductCodes.AnyAsync(productCode => (productCode.Manufacturer.Code == manufacturerCode && productCode.Code == code));
        }

        public async Task<bool> ProductCodeExistsAsync(long id)
        {
            return await context.ProductCodes.AnyAsync(productCode => (productCode.Id == id));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<ProductCode> UpdateProductCodeAsync(ProductCode productCode)
        {
            if (productCode is not null)
            {
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
            }

            return null;
        }
    }
}
