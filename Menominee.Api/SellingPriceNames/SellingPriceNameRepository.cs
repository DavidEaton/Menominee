using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.SellingPriceNames;

public class SellingPriceNameRepository : ISellingPriceNameRepository
{
    private readonly ApplicationDbContext context;

    public SellingPriceNameRepository(ApplicationDbContext context)
    {
        this.context = context ??
            throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyList<SellingPriceName>> GetAllAsync()
    {
        return await context.SellingPriceNames
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<SellingPriceName> GetEntityAsync(long id)
    {
        return await context.SellingPriceNames
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(SellingPriceName sellingPriceName)
    {
        if (sellingPriceName is not null)
            context.Attach(sellingPriceName);
    }

    public void Delete(SellingPriceName sellingPriceName)
    {
        if (sellingPriceName is not null)
            context.Remove(sellingPriceName);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
