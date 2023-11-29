using CSharpFunctionalExtensions;
using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.SaleCodes
{
    public class SaleCodeRepository : ISaleCodeRepository
    {
        private readonly ApplicationDbContext context;

        public SaleCodeRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(SaleCode saleCode)
        {
            if (saleCode is not null)
                context.Attach(saleCode);
        }

        public void Delete(SaleCode saleCode)
        {
            if (saleCode is not null)
                context.Remove(saleCode);
        }

        public async Task DeleteSaleCodeAsync(long id)
        {
            var saleCodeFromContext = await context.SaleCodes.FindAsync(id);
            if (saleCodeFromContext != null)
                context.Remove(saleCodeFromContext);
        }


        public async Task<SaleCodeToRead> GetAsync(long id)
        {
            return SaleCodeHelper.ConvertToReadDto(await context.SaleCodes
                .Include(saleCode => saleCode.ShopSupplies)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(saleCode => saleCode.Id == id));
        }

        public async Task<Result<IReadOnlyList<SaleCode>>> GetSaleCodeEntitiesAsync(List<long> ids, bool all = false)
        {
            var saleCodes = all
                ? await context.SaleCodes.ToListAsync()
                : await context.SaleCodes
                    .Where(saleCode => ids.Contains(saleCode.Id))
                    .ToListAsync();

            if (saleCodes == null || !saleCodes.Any())
                return Result.Failure<IReadOnlyList<SaleCode>>("No sale codes found.");

            return Result.Success<IReadOnlyList<SaleCode>>(saleCodes);
        }

        public async Task<SaleCode> GetEntityAsync(string code)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Code == code);

            return saleCodeFromContext;
        }

        public async Task<SaleCode> GetEntityAsync(long id)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Id == id);

            return saleCodeFromContext;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetListAsync()
        {
            var saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes
                .Select(saleCode => SaleCodeHelper.ConvertToReadInListDto(saleCode))
                .ToList();
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToRead>> GetShopSuppliesAsync(long Id)
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

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetShopSuppliesListAsync()
        {
            return await context.SaleCodes
                .Include(saleCode => saleCode.ShopSupplies)
                .Select(saleCode => SaleCodeHelper.ConvertShopSuppliesToReadInListDto(saleCode))
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
