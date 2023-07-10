using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.SaleCodes
{
    public class SaleCodeRepository : ISaleCodeRepository
    {
        private readonly ApplicationDbContext context;

        public SaleCodeRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddSaleCodeAsync(SaleCode saleCode)
        {
            if (saleCode != null)
                await context.AddAsync(saleCode);
        }

        public async Task DeleteSaleCodeAsync(long id)
        {
            var saleCodeFromContext = await context.SaleCodes.FindAsync(id);
            if (saleCodeFromContext != null)
                context.Remove(saleCodeFromContext);
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(string code)
        {
            return SaleCodeHelper.ConvertToReadDto(await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Code == code));
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(long id)
        {
            return SaleCodeHelper.ConvertToReadDto(
                await context.SaleCodes.FirstOrDefaultAsync(
                    saleCode => saleCode.Id == id));
        }

        public async Task<IReadOnlyList<SaleCode>> GetSaleCodeEntitiesAsync(List<long> ids)
        {
            return await context.SaleCodes
                .Where(saleCode => ids.Contains(saleCode.Id))
                .ToListAsync();
        }

        public async Task<SaleCode> GetSaleCodeEntityAsync(string code)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Code == code);

            return saleCodeFromContext;
        }

        public async Task<SaleCode> GetSaleCodeEntityAsync(long id)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Id == id);

            return saleCodeFromContext;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetSaleCodeListAsync()
        {
            var saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes
                .Select(saleCode => SaleCodeHelper.ConvertToReadInListDto(saleCode))
                .ToList();
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToRead>> GetSaleCodeShopSupplies(long Id)
        {
            var shopSupplies = await context.SaleCodeShopSupplies.ToListAsync();

            return shopSupplies
                .Select(shopSupply => new SaleCodeShopSuppliesToRead()
                {
                    Id = shopSupply.Id,
                    Percentage = shopSupply.Percentage,
                    IncludeLabor = shopSupply.IncludeLabor,
                    IncludeParts = shopSupply.IncludeParts,
                    MaximumCharge = shopSupply.MaximumCharge,
                    MinimumCharge = shopSupply.MinimumCharge,
                    MinimumJobAmount = shopSupply.MinimumJobAmount
                })
                .ToList();
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetSaleCodeShopSuppliesListAsync()
        {
            var saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes
                .Select(saleCode => SaleCodeHelper.ConvertShopSuppliesToReadInListDto(saleCode))
                .ToList();
        }

        public async Task<bool> SaleCodeExistsAsync(long id)
        {
            return await context.SaleCodes.AnyAsync(saleCode => saleCode.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
