using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.ProductCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menominee.Api.ProductCodes
{
    public class ProductCodeRepository : IProductCodeRepository
    {
        private readonly ApplicationDbContext context;

        public ProductCodeRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        private IQueryable<ProductCode> GetProductCodesWithIncludes(bool asNoTracking = false)
        {
            var query = context.ProductCodes
                .Include(productCode => productCode.Manufacturer)
                .Include(productCode => productCode.SaleCode)
                .AsSplitQuery();

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }


        private ProductCodeToRead ConvertToReadDto(ProductCode productCode)
        {
            return productCode is not null
                ? ProductCodeHelper.ConvertToReadDto(productCode)
                : null;
        }

        public void AddProductCode(ProductCode productCode)
        {
            if (productCode is not null)
                context.Attach(productCode);
        }

        public async Task DeleteProductCode(long id)
        {
            var productCodeFromContext = await context.ProductCodes.FindAsync(id);

            if (productCodeFromContext is not null)
            {
                context.Remove(productCodeFromContext);
                context.SaveChanges();
            }
        }

        public IReadOnlyList<string> GetManufacturerCodes()
        {
            return context.ProductCodes.Select(productCode => $"{productCode.Manufacturer.Id}{productCode.Code}").ToList();
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(long manufacturerId, string code)
        {
            return await GetProductCodeAsync(productCode =>
                productCode.Manufacturer.Id == manufacturerId
                &&
                productCode.Code == code);
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(long Id)
        {
            return await GetProductCodeAsync(productCode => productCode.Id == Id);
        }

        private async Task<ProductCodeToRead> GetProductCodeAsync(Expression<Func<ProductCode, bool>> predicate)
        {
            var productCodeFromContext = await GetProductCodesWithIncludes(asNoTracking: true)
                .FirstOrDefaultAsync(predicate);

            return ConvertToReadDto(productCodeFromContext);
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(long manufacturerId, string code)
        {
            return await context.ProductCodes
                .Include(productCode => productCode.Manufacturer)
                .Include(productCode => productCode.SaleCode)
                .AsSplitQuery()
                .FirstOrDefaultAsync(productCode =>
                    productCode.Manufacturer.Id == manufacturerId
                    &&
                    productCode.Code == code);
        }

        public async Task<ProductCode> GetProductCodeEntityAsync(long id)
        {
            var result = await GetProductCodesWithIncludes(asNoTracking: false)
                .FirstOrDefaultAsync(productCode => productCode.Id == id);

            return result;
        }

        public async Task<IReadOnlyList<ProductCodeToRead>> GetProductCodes()
        {
            var query = await GetProductCodesWithIncludes(asNoTracking: true).ToArrayAsync();

            return query
                .Select(productCode => ConvertToReadDto(productCode))
                .ToList();
        }

        public async Task<bool> ProductCodeExists(long id)
        {
            return await context.ProductCodes.AnyAsync(productCode => productCode.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetProductCodesInListAsync(long? manufacturerId, long? saleCodeId)
        {
            var query = GetProductCodesWithIncludes(asNoTracking: true);

            if (manufacturerId is not null)
                query = query.Where(productCode => productCode.Manufacturer.Id == manufacturerId);

            if (saleCodeId is not null)
                query = query.Where(productCode => productCode.SaleCode.Id == saleCodeId);

            return await query
                .Select(productCode => ProductCodeHelper.ConvertToReadInListDto(productCode))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductCode>> GetProductCodeEntitiesAsync()
        {
            return await GetProductCodesWithIncludes(asNoTracking: false).ToListAsync();
        }
    }
}
