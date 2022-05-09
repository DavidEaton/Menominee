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
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemHelper.CreateInventoryItem(itemFromContext);
        }

        public async Task<InventoryItemToRead> GetItemAsync(long manufacturerId, string itemNumber)
        {
            var itemFromContext = await context.InventoryItems
                                               .Include(item => item.Manufacturer)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(productCode => productCode.SaleCode)
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.ManufacturerId == manufacturerId
                                                                         && item.ItemNumber == itemNumber);

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemHelper.CreateInventoryItem(itemFromContext);
        }

        public async Task<InventoryItem> GetItemEntityAsync(long id)
        {
            return await context.InventoryItems
                                .Include(item => item.Manufacturer)
                                .Include(item => item.ProductCode)
                                    .ThenInclude(productCode => productCode.SaleCode)
                                .Include(item => item.Part)
                                .Include(item => item.Labor)
                                .Include(item => item.Tire)
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
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            foreach (var item in itemsFromContext)
                items.Add(InventoryItemHelper.CreateInventoryItem(item));

            return items;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync()
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.SaleCode)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => InventoryItemHelper.CreateInventoryItemInList(item))
                                   .ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync(long manufacturerId)
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(productCode => productCode.SaleCode)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .Where(item => item.ManufacturerId == manufacturerId)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => InventoryItemHelper.CreateInventoryItemInList(item))
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

