using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    public class SalesTaxRepository : ISalesTaxRepository
    {
        private readonly ApplicationDbContext context;

        public SalesTaxRepository(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddSalesTaxAsync(SalesTax salesTax)
        {
            Guard.ForNull(salesTax, "Sales Tax");

            if (await SalesTaxExistsAsync(salesTax.Id))
                throw new Exception("Sales Tax already exists");

            await context.AddAsync(salesTax);
        }

        public async Task DeleteSalesTaxAsync(long id)
        {
            var salesTax = await context.SalesTaxes
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(tax => tax.Id == id);

            Guard.ForNull(salesTax, "Sales Tax");

            context.Remove(salesTax);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<SalesTaxToRead> GetSalesTaxAsync(long id)
        {
            var taxFromContext = await context.SalesTaxes
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(tax => tax.Id == id);

            Guard.ForNull(taxFromContext, "Sales Tax");

            return SalesTaxHelper.Transform(taxFromContext);
        }

        public async Task<SalesTax> GetSalesTaxEntityAsync(long id)
        {
            return await context.SalesTaxes.FirstOrDefaultAsync(tax => tax.Id == id);
        }

        public async Task<IReadOnlyList<SalesTaxToReadInList>> GetSalesTaxListAsync()
        {
            IReadOnlyList<SalesTax> salesTaxes = await context.SalesTaxes.ToListAsync();

            return salesTaxes
                .Select(tax => SalesTaxHelper.TransformToListItem(tax))
                .ToList();
        }

        public async Task<bool> SalesTaxExistsAsync(long id)
        {
            return await context.SalesTaxes.AnyAsync(tax => tax.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<SalesTax> UpdateSalesTaxAsync(SalesTax salesTax)
        {
            Guard.ForNull(salesTax, "Sales Tax");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(salesTax).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SalesTaxExistsAsync(salesTax.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
