using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Inventory
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ApplicationDbContext context;

        public InventoryItemRepository(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(InventoryItem item)
        {
            if (item is not null)
                context.Attach(item);
        }


        public void Delete(InventoryItem item)
        {
            if (item is not null)
                context.Remove(item);
        }

        public async Task<InventoryItemToRead> GetAsync(long id) =>
            await GetInventoryItemToRead(item => item.Id == id);

        public async Task<InventoryItemToRead> GetAsync(long manufacturerId, string itemNumber) =>
            await GetInventoryItemToRead(item => item.Manufacturer.Id == manufacturerId && item.ItemNumber == itemNumber);

        public async Task<InventoryItem> GetEntityAsync(long id) =>
            await GetInventoryItemEntity(item => item.Id == id);

        public async Task<IReadOnlyList<InventoryItemToRead>> GetAllAsync() =>
            await GetInventoryItemsToRead();

        public async Task<IReadOnlyList<InventoryItem>> GetEntitiesAsync(List<long> ids) =>
            await Task.WhenAll(ids.Select(id => GetEntityAsync(id)));
        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetListAsync() =>
            await GetInventoryItemsToReadInList();

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetListAsync(long manufacturerId) =>
            await GetInventoryItemsToReadInList(manufacturerId: manufacturerId);

        private IQueryable<InventoryItem> GetInventoryItemsQuery(bool asNoTracking = true)
        {
            var query = context.InventoryItems
                .Include(item => item.Manufacturer)
                .Include(item => item.ProductCode)
                    .ThenInclude(productCode => productCode.SaleCode)
                .Include(item => item.ProductCode)
                    .ThenInclude(productCode => productCode.Manufacturer)
                .Include(item => item.Part)
                    .ThenInclude(part => part.ExciseFees)
                .Include(item => item.Labor)
                .Include(item => item.Tire)
                    .ThenInclude(tire => tire.ExciseFees)
                .Include(item => item.Package)
                    .ThenInclude(package => package.Items)
                        .ThenInclude(packageItem => packageItem.Item.Manufacturer)
                .Include(item => item.Package)
                    .ThenInclude(package => package.Items)
                        .ThenInclude(packageItem => packageItem.Item.ProductCode)
                            .ThenInclude(productCode => productCode.SaleCode)
                .Include(item => item.Package)
                    .ThenInclude(packageItem => packageItem.Items)
                        .ThenInclude(pItem => pItem.Item)
                            .ThenInclude(item => item.Part)
                .Include(item => item.Package)
                    .ThenInclude(package => package.Items)
                        .ThenInclude(packageItem => packageItem.Item)
                            .ThenInclude(item => item.Labor)
                .Include(item => item.Package)
                    .ThenInclude(package => package.Items)
                        .ThenInclude(packageItem => packageItem.Item)
                            .ThenInclude(item => item.Tire)
                .Include(item => item.Package)
                    .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                .Include(item => item.Inspection)
                .Include(item => item.Warranty)
                .AsSplitQuery();

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        private async Task<InventoryItemToRead> GetInventoryItemToRead(Expression<Func<InventoryItem, bool>> predicate) =>
            await GetInventoryItemsQuery()
                .FirstOrDefaultAsync(predicate) is InventoryItem itemFromContext
                ? InventoryItemHelper.ConvertToReadDto(itemFromContext)
                : null;

        private async Task<InventoryItem> GetInventoryItemEntity(Expression<Func<InventoryItem, bool>> predicate) =>
            await GetInventoryItemsQuery(asNoTracking: false)
                .FirstOrDefaultAsync(predicate);

        private async Task<List<InventoryItem>> GetInventoryItemsQueryAsync(InventoryItemType? itemType = null, string itemNumber = null, long? manufacturerId = null, long? productCodeId = null)
        {
            var query = GetInventoryItemsQuery();

            if (manufacturerId is not null)
                query = query.Where(item => item.Manufacturer.Id == manufacturerId);

            if (itemNumber is not null)
                query = query.Where(item => item.ItemNumber == itemNumber);

            if (productCodeId is not null)
                query = query.Where(item => item.ProductCode.Id == productCodeId);

            if (itemType is not null)
                query = query.Where(item => item.ItemType == itemType);

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryItemToRead>> GetInventoryItemsToRead(string partNumber = null, long? manufacturerId = null, long? productCodeId = null, InventoryItemType? itemType = null)
        {
            var itemsFromContext = await GetInventoryItemsQueryAsync(itemType, partNumber, manufacturerId, productCodeId);

            return itemsFromContext.Select(item => InventoryItemHelper.ConvertToReadDto(item)).ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemsToReadInList(InventoryItemType? itemType = null, string itemNumber = null, long? manufacturerId = null, long? productCodeId = null)
        {
            var itemsFromContext = await GetInventoryItemsQueryAsync(itemType, itemNumber, manufacturerId, productCodeId);

            return itemsFromContext.Select(item => InventoryItemHelper.ConvertToReadInListDto(item)).ToList();
        }

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();

        public async Task<InventoryItemWarranty> GetWarrantyEntityAsync(long id) =>
            await context.InventoryItemWarranties
                .FirstOrDefaultAsync(warranty => warranty.Id == id);

        public async Task<InventoryItemPart> GetPartEntityAsync(long id) =>
            await context.InventoryItemParts
                .FirstOrDefaultAsync(part => part.Id == id);

        public async Task<InventoryItemInspection> GetInspectionEntityAsync(long id) =>
            await context.InventoryItemInspections
                .FirstOrDefaultAsync(inspection => inspection.Id == id);

        public async Task<InventoryItemLabor> GetLaborEntityAsync(long id) =>
            await context.InventoryItemLabor
                .FirstOrDefaultAsync(labor => labor.Id == id);

        public async Task<InventoryItemTire> GetTireEntityAsync(long id) =>
            await context.InventoryItemTires
                .FirstOrDefaultAsync(tire => tire.Id == id);

        public async Task<InventoryItemPackage> GetPackageEntityAsync(long id) =>
            await context.InventoryItemPackages
                .FirstOrDefaultAsync(package => package.Id == id);
    }
}
