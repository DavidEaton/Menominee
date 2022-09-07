using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.SaleCodes
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

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(string code)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(sc => sc.Code == code);

            return SaleCodeHelper.ConvertEntityToReadDto(saleCodeFromContext);
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(long id)
        {
            var saleCodeFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(saleCode => saleCode.Id == id);

            return SaleCodeHelper.ConvertEntityToReadDto(saleCodeFromContext);
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
            IReadOnlyList<SaleCode> saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes
                .Select(saleCode => SaleCodeHelper.ConvertEntityToReadInListDto(saleCode))
                .ToList();
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetSaleCodeShopSuppliesListAsync()
        {
            IReadOnlyList<SaleCode> saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes
                .Select(saleCode => SaleCodeHelper.ConvertEntityToShopSuppliesToReadInListDto(saleCode))
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
