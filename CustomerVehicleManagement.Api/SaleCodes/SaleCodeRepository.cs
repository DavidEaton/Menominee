using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
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

        public async Task DeleteSaleCodeAsync(string code)
        {
            var scFromContext = await context.SaleCodes.FindAsync(code);
            if (scFromContext != null)
                context.Remove(scFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(string code)
        {
            var scFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(sc => sc.Code == code);

            return RepairOrderHelper.TransformSaleCode(scFromContext);
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(long id)
        {
            var scFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(sc => sc.Id == id);

            return RepairOrderHelper.TransformSaleCode(scFromContext);
        }

        public async Task<SaleCode> GetSaleCodeEntityAsync(string code)
        {
            var scFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(sc => sc.Code == code);

            return scFromContext;
        }

        public async Task<SaleCode> GetSaleCodeEntityAsync(long id)
        {
            var scFromContext = await context.SaleCodes
                .FirstOrDefaultAsync(sc => sc.Id == id);

            return scFromContext;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetSaleCodeListAsync()
        {
            IReadOnlyList<SaleCode> saleCodes = await context.SaleCodes.ToListAsync();

            return saleCodes.
                Select(sc => SaleCodeToReadInList.ConvertToDto(sc))
                .ToList();
        }

        public async Task<bool> SaleCodeExistsAsync(string code)
        {
            return await context.SaleCodes.AnyAsync(sc => sc.Code == code);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateSaleCodeAsync(SaleCode saleCode)
        {
            // No code in this implementation
        }
    }
}
