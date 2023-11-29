using Menominee.Api.Data;
using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Taxes
{
    public class SalesTaxRepository : ISalesTaxRepository
    {
        private readonly ApplicationDbContext context;

        public SalesTaxRepository(ApplicationDbContext context)
        {
            if (context is not null)
                this.context = context;
        }

        public void Add(SalesTax salesTax)
        {
            if (salesTax is not null)
                context.Attach(salesTax);
        }

        public void Delete(SalesTax salesTax)
        {
            context.Remove(salesTax);
        }

        public async Task<SalesTaxToRead> GetAsync(long id)
        {
            var taxFromContext = await context.SalesTaxes
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(tax => tax.Id == id);

            return taxFromContext is not null
                ? SalesTaxHelper.ConvertToReadDto(taxFromContext)
                : null;
        }

        public async Task<IReadOnlyList<SalesTax>> GetEntitiesAsync()
        {
            return await context.SalesTaxes.ToListAsync();
        }

        public async Task<SalesTax> GetEntityAsync(long id)
        {
            return await context.SalesTaxes.FirstOrDefaultAsync(tax => tax.Id == id);
        }

        public async Task<IReadOnlyList<SalesTaxToRead>> GetAllAsync()
        {
            IReadOnlyList<SalesTax> salesTaxes = await context.SalesTaxes.ToListAsync();

            return salesTaxes
                .Select(tax => SalesTaxHelper.ConvertToReadDto(tax))
                .ToList();
        }

        public async Task<IReadOnlyList<SalesTaxToReadInList>> GetListAsync()
        {
            IReadOnlyList<SalesTax> salesTaxes = await context.SalesTaxes.ToListAsync();

            return salesTaxes
                .Select(tax => SalesTaxHelper.ConvertToReadInListDto(tax))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
