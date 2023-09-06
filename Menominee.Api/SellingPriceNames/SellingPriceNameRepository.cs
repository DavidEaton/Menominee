using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<IReadOnlyList<SellingPriceName>> GetAll()
    {
        return await context.SellingPriceNames
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<SellingPriceName> GetEntity(long id)
    {
        return await context.SellingPriceNames
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(SellingPriceName entity)
    {
        var existingEntity = context.SellingPriceNames.Local
            .FirstOrDefault(sellingPriceName => sellingPriceName.Id.Equals(entity.Id));

        if (existingEntity is not null)
        {
            context.Entry(existingEntity).State = EntityState.Detached;
        }

        context.SellingPriceNames.Attach(entity);
    }

    public void Delete(SellingPriceName entity)
    {
        context.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}
