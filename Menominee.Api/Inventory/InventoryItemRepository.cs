using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ApplicationDbContext context;

        public InventoryItemRepository(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(InventoryItem item)
        {
            if (item is not null)
            {
                if (await Exists(item.Id))
                    throw new Exception("Inventory Item already exists");

                // Detach any existing tracked entity with the same key
                if (item.Manufacturer is not null)
                {
                    var existingManufacturer = context.Manufacturers.Local.FirstOrDefault(e => e.Id == item.Manufacturer.Id);
                    if (existingManufacturer is not null)
                        context.Entry(existingManufacturer).State = EntityState.Detached;
                }

                context.InventoryItems.Attach(item);
            }
        }

        public void Delete(InventoryItem item)
        {
            if (item is not null)
            {
                context.Remove(item);
                context.SaveChanges();
            }
        }

        public async Task<InventoryItemToRead> GetItem(long id) =>
            await GetInventoryItemToRead(item => item.Id == id);

        public async Task<InventoryItemToRead> GetItem(long manufacturerId, string itemNumber) =>
            await GetInventoryItemToRead(item => item.Manufacturer.Id == manufacturerId && item.ItemNumber == itemNumber);

        public async Task<InventoryItem> GetItemEntity(long id) =>
            await GetInventoryItemEntity(item => item.Id == id);

        public async Task<IReadOnlyList<InventoryItemToRead>> GetItems() =>
            await GetInventoryItemsToRead();

        public async Task<IReadOnlyList<InventoryItem>> GetInventoryItemEntities(List<long> ids) =>
            await Task.WhenAll(ids.Select(id => GetItemEntity(id)));

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInList() =>
            await GetInventoryItemsToReadInList();

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInList(long manufacturerId) =>
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
                .Include(item => item.Labor)
                .Include(item => item.Tire)
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

        private async Task<IReadOnlyList<InventoryItemToRead>> GetInventoryItemsToRead(string partNumber = null, long? manufacturerId = null, long? productCodeId = null, InventoryItemType? itemType = null)
        {
            IQueryable<InventoryItem> query = GetInventoryItemsQuery();

            if (manufacturerId is not null)
                query = query.Where(item => item.Manufacturer.Id == manufacturerId);

            if (partNumber is not null)
                query = query.Where(item => item.ItemNumber == partNumber);

            if (productCodeId is not null)
                query = query.Where(item => item.ProductCode.Id == productCodeId);

            if (itemType is not null)
                query = query.Where(item => item.ItemType == itemType);

            var itemsFromContext = await query.ToListAsync();

            return itemsFromContext.Select(
                item => InventoryItemHelper.ConvertToReadDto(item))
                .ToList();
        }

        private async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemsToReadInList(InventoryItemType? itemType = null, string itemNumber = null, long? manufacturerId = null, long? productCodeId = null)
        {
            IQueryable<InventoryItem> query = GetInventoryItemsQuery();

            if (manufacturerId is not null)
                query = query.Where(
                    item => item.Manufacturer.Id == manufacturerId);

            if (itemNumber is not null)
                query = query.Where(
                    item => item.ItemNumber == itemNumber);

            if (productCodeId is not null)
                query = query.Where(
                    item => item.ProductCode.Id == productCodeId);

            if (itemType is not null)
                query = query.Where(
                    item => item.ItemType == itemType);

            var itemsFromContext = await query.ToListAsync();

            return itemsFromContext.Select(
                item => InventoryItemHelper.ConvertToReadInListDto(item))
                .ToList();
        }

        public async Task<bool> Exists(long id) =>
            await context.InventoryItems.AnyAsync(item => item.Id == id);

        public async Task SaveChanges() =>
            await context.SaveChangesAsync();

        public async Task<InventoryItemWarranty> GetInventoryItemWarrantyEntity(long id) =>
            await context.InventoryItemWarranties
                .FirstOrDefaultAsync(warranty => warranty.Id == id);

        public async Task<InventoryItemPart> GetInventoryItemPartEntity(long id) =>
            await context.InventoryItemParts
                .FirstOrDefaultAsync(part => part.Id == id);

        public async Task<InventoryItemInspection> GetInventoryItemInspectionEntity(long id) =>
            await context.InventoryItemInspections
                .FirstOrDefaultAsync(inspection => inspection.Id == id);

        public async Task<InventoryItemLabor> GetInventoryItemLaborEntity(long id) =>
            await context.InventoryItemLabor
                .FirstOrDefaultAsync(labor => labor.Id == id);

        public async Task<InventoryItemTire> GetInventoryItemTireEntity(long id) =>
            await context.InventoryItemTires
                .FirstOrDefaultAsync(tire => tire.Id == id);

        public async Task<InventoryItemPackage> GetInventoryItemPackageEntity(long id) =>
            await context.InventoryItemPackages
                .FirstOrDefaultAsync(package => package.Id == id);
    }
}
