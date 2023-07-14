﻿using Menominee.Api.Data;
using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Taxes
{
    public class SalesTaxRepository : ISalesTaxRepository
    {
        private readonly ApplicationDbContext context;

        public SalesTaxRepository(ApplicationDbContext context)
        {
            if (context is not null)
                this.context = context;
        }

        public async Task AddSalesTaxAsync(SalesTax salesTax)
        {
            if (salesTax is not null)
            {
                if (await SalesTaxExistsAsync(salesTax.Id))
                    throw new Exception("Sales Tax already exists");

                await context.AddAsync(salesTax);
            }
        }

        public async Task DeleteSalesTaxAsync(long id)
        {
            var salesTax = await context.SalesTaxes
                                         .FirstOrDefaultAsync(tax => tax.Id == id);

            if (salesTax is not null)
                context.Remove(salesTax);
        }

        public async Task<SalesTaxToRead> GetSalesTaxAsync(long id)
        {
            var taxFromContext = await context.SalesTaxes
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(tax => tax.Id == id);

            return taxFromContext is not null
                ? SalesTaxHelper.ConvertToReadDto(taxFromContext)
                : null;
        }

        public async Task<IReadOnlyList<SalesTax>> GetSalesTaxEntities()
        {
            return await context.SalesTaxes.ToListAsync();
        }

        public async Task<SalesTax> GetSalesTaxEntityAsync(long id)
        {
            return await context.SalesTaxes.FirstOrDefaultAsync(tax => tax.Id == id);
        }

        public async Task<IReadOnlyList<SalesTaxToRead>> GetSalesTaxesAsync()
        {
            IReadOnlyList<SalesTax> salesTaxes = await context.SalesTaxes.ToListAsync();

            return salesTaxes
                .Select(tax => SalesTaxHelper.ConvertToReadDto(tax))
                .ToList();
        }

        public async Task<IReadOnlyList<SalesTaxToReadInList>> GetSalesTaxListAsync()
        {
            IReadOnlyList<SalesTax> salesTaxes = await context.SalesTaxes.ToListAsync();

            return salesTaxes
                .Select(tax => SalesTaxHelper.ConvertToReadInListDto(tax))
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
            if (salesTax is not null)
            {
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
            }

            return null;
        }
    }
}