using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ApplicationDbContext context;

        public InventoryItemRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddItemAsync(InventoryItem item)
        {
            if (item is not null)
            {
                if (await ItemExistsAsync(item.Id))
                    throw new Exception("Inventory Item already exists");

                await context.AddAsync(item);
            }
        }

        public void DeleteInventoryItem(InventoryItem item)
        {
            if (item is not null)
            {
                context.Remove(item);
                context.SaveChanges();
            }
        }

        public async Task DeleteItemAsync(long id)
        {
            var item = await context.InventoryItems
                                    .FirstOrDefaultAsync(item => item.Id == id);

            if (item is not null)
            {
                context.Remove(item);
                context.SaveChanges();
            }
        }

        public async Task<InventoryItemToRead> GetItemAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                                               .Include(item => item.Manufacturer)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(productCode => productCode.SaleCode)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(productCode => productCode.Manufacturer)
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                               //.Include(item => item.Package)
                                               //    .ThenInclude(pkgItem => pkgItem.Items)
                                               //        .ThenInclude(pItem => pItem.Item)
                                               //            .ThenInclude(item => item.Manufacturer)
                                               //.Include(item => item.Package)
                                               //    .ThenInclude(pkgItem => pkgItem.Items)
                                               //        .ThenInclude(pItem => pItem.Item)
                                               //            .ThenInclude(item => item.ProductCode)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem)
                                                           .ThenInclude(item => item.Part)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem)
                                                           .ThenInclude(item => item.Labor)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem)
                                                           .ThenInclude(item => item.Tire)
                                               //.Include(item => item.Package)
                                               //    .ThenInclude(pkgItem => pkgItem.Items)
                                               //        .ThenInclude(invItem => invItem.Item)
                                               //            .ThenInclude(item => item.Manufacturer)
                                               //.Include(item => item.Package)
                                               //    .ThenInclude(pkgItem => pkgItem.Items)
                                               //        .ThenInclude(invItem => invItem.Item)
                                               //            .ThenInclude(item => item.ProductCode)
                                               //                .ThenInclude(productCode => productCode.SaleCode)
                                               .Include(item => item.Package)
                                                   .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                               .Include(item => item.Inspection)
                                               // Coming soon...
                                               //.Include(item => item.Donation)
                                               //.Include(item => item.GiftCertificate)
                                               .Include(item => item.Warranty)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            return itemFromContext is not null
                ? InventoryItemHelper.ConvertEntityToReadDto(itemFromContext)
                : null;
        }

        public async Task<InventoryItemToRead> GetItemAsync(long manufacturerId, string itemNumber)
        {
            var itemFromContext = await context.InventoryItems
                                               .Include(item => item.Manufacturer)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(productCode => productCode.SaleCode)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(productCode => productCode.Manufacturer)
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                               .Include(item => item.Package)
                                                   .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                               .Include(item => item.Inspection)
                                               // Coming soon...
                                               //.Include(item => item.Donation)
                                               //.Include(item => item.GiftCertificate)
                                               .Include(item => item.Warranty)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Manufacturer.Id == manufacturerId
                                                                         && item.ItemNumber == itemNumber);

            return itemFromContext is not null
                ? InventoryItemHelper.ConvertEntityToReadDto(itemFromContext)
                : null;
        }

        public async Task<InventoryItem> GetItemEntityAsync(long id)
        {
            return await context.InventoryItems
                                .Include(item => item.Manufacturer)
                                .Include(item => item.ProductCode)
                                    .ThenInclude(productCode => productCode.SaleCode)
                                .Include(item => item.ProductCode)
                                    .ThenInclude(productCode => productCode.Manufacturer)
                                .Include(item => item.Part)
                                .Include(item => item.Labor)
                                .Include(item => item.Tire)
                                .Include(item => item.Package)
                                    .ThenInclude(pkgItem => pkgItem.Items)
                                .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                    .Include(item => item.Package)
                                .ThenInclude(pkgItem => pkgItem.Items)
                                    .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                .Include(item => item.Package)
                                    .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                .Include(item => item.Inspection)
                                // Coming soon...
                                //.Include(item => item.Donation)
                                //.Include(item => item.GiftCertificate)
                                .Include(item => item.Warranty)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<InventoryItem>> GetInventoryItemEntitiesAsync(List<long> ids)
        {
            var list = new List<InventoryItem>();

            foreach (var id in ids)
                list.Add(await GetItemEntityAsync(id));

            return list;
        }

        public async Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync()
        {
            var items = new List<InventoryItemToRead>();

            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.SaleCode)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.Manufacturer)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                                .Include(item => item.Package)
                                                    .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                               .Include(item => item.Inspection)
                                                // Coming soon...
                                                //.Include(item => item.Donation)
                                                //.Include(item => item.GiftCertificate)
                                                .Include(item => item.Warranty)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            foreach (var item in itemsFromContext)
                items.Add(InventoryItemHelper.ConvertEntityToReadDto(item));

            return items;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync()
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.SaleCode)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.Manufacturer)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                                .Include(item => item.Package)
                                                    .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                               .Include(item => item.Inspection)
                                                // Coming soon...
                                                //.Include(item => item.Donation)
                                                //.Include(item => item.GiftCertificate)
                                                .Include(item => item.Warranty)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => InventoryItemHelper.ConvertEntityToReadInListDto(item))
                                   .ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync(long manufacturerId)
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.SaleCode)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.Manufacturer)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.InventoryItem.ProductCode)
                                                .Include(item => item.Package)
                                                    .ThenInclude(placeholderItem => placeholderItem.Placeholders)
                                               .Include(item => item.Inspection)
                                                // Coming soon...
                                                //.Include(item => item.Donation)
                                                //.Include(item => item.GiftCertificate)
                                                .Include(item => item.Warranty)
                                                .Where(item => item.Manufacturer.Id == manufacturerId)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => InventoryItemHelper.ConvertEntityToReadInListDto(item))
                                   .ToList();
        }

        public async Task<bool> ItemExistsAsync(long id)
        {
            return await context.InventoryItems.AnyAsync(item => item.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            if (item is not null)
            {
                // Tracking IS needed for commands for disconnected data collections
                context.Entry(item).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ItemExistsAsync(item.Id))
                        return null;// something that tells the controller to return NotFound();
                    throw;
                }
            }

            return null;
        }

        public async Task<InventoryItemWarranty> GetInventoryItemWarrantyEntityAsync(long id)
        {
            return await context.InventoryItemWarranties
                                .FirstOrDefaultAsync(warranty => warranty.Id == id);
        }

        public async Task<InventoryItemPart> GetInventoryItemPartEntityAsync(long id)
        {
            return await context.InventoryItemParts
                                .FirstOrDefaultAsync(part => part.Id == id);
        }

        public async Task<InventoryItemInspection> GetInventoryItemInspectionEntityAsync(long id)
        {
            return await context.InventoryItemInspections
                                .FirstOrDefaultAsync(part => part.Id == id);
        }

        public async Task<InventoryItemLabor> GetInventoryItemLaborEntityAsync(long id)
        {
            return await context.InventoryItemLabor
                                .FirstOrDefaultAsync(part => part.Id == id);
        }

        public async Task<InventoryItemTire> GetInventoryItemTireEntityAsync(long id)
        {
            return await context.InventoryItemTires
                                .FirstOrDefaultAsync(part => part.Id == id);
        }

        public async Task<InventoryItemPackage> GetInventoryItemPackageEntityAsync(long id)
        {
            return await context.InventoryItemPackages
                                .FirstOrDefaultAsync(part => part.Id == id);
        }
    }
}

