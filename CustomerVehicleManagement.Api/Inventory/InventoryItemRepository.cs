using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Utilities;
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
            Guard.ForNull(item, "item");

            if (await ItemExistsAsync(item.Id))
                throw new Exception("Inventory Item already exists");

            await context.AddAsync(item);
        }

        public void DeleteInventoryItem(InventoryItem item)
        {
            Guard.ForNull(item, "item");

            context.Remove(item);
            context.SaveChanges();
        }

        public async Task DeleteItemAsync(long id)
        {
            var item = await context.InventoryItems
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(item, "item");

            context.Remove(item);
            context.SaveChanges();
        }

        public void FixTrackingState()
        {
            context.FixState();
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
                                                       .ThenInclude(pItem => pItem.Item.Manufacturer)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.Item.ProductCode)
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
                                                       .ThenInclude(pItem => pItem.Item)
                                                           .ThenInclude(item => item.Part)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.Item)
                                                           .ThenInclude(item => item.Labor)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.Item)
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

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemHelper.ConvertEntityToReadDto(itemFromContext);
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
                                                       .ThenInclude(pItem => pItem.Item.Manufacturer)
                                               .Include(item => item.Package)
                                                   .ThenInclude(pkgItem => pkgItem.Items)
                                                       .ThenInclude(pItem => pItem.Item.ProductCode)
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

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemHelper.ConvertEntityToReadDto(itemFromContext);
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
                                .ThenInclude(pItem => pItem.Item.Manufacturer)
                                    .Include(item => item.Package)
                                .ThenInclude(pkgItem => pkgItem.Items)
                                    .ThenInclude(pItem => pItem.Item.ProductCode)
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
                                                        .ThenInclude(pItem => pItem.Item.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.Item.ProductCode)
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
                                                        .ThenInclude(pItem => pItem.Item.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.Item.ProductCode)
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
                                                        .ThenInclude(pItem => pItem.Item.Manufacturer)
                                                .Include(item => item.Package)
                                                    .ThenInclude(pkgItem => pkgItem.Items)
                                                        .ThenInclude(pItem => pItem.Item.ProductCode)
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

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            Guard.ForNull(item, "item");

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

            return null;
        }
    }
}

