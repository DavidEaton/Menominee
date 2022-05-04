using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Utilities;
using Menominee.Common.Enums;
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
        public async Task DeleteItemAsync(long id)
        {
            context.Remove(item);
            var item = await context.InventoryItems
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(item, "item");

            context.Remove(item);
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
                                                   .ThenInclude(pc => pc.SaleCode)
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemToRead.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItemToRead> GetItemAsync(long mfrId, string itemNumber)
        {
            var itemFromContext = await context.InventoryItems
                                               .Include(item => item.Manufacturer)
                                               .Include(item => item.ProductCode)
                                                   .ThenInclude(pc => pc.SaleCode)
                                               .Include(item => item.Part)
                                               .Include(item => item.Labor)
                                               .Include(item => item.Tire)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(item => item.ManufacturerId == mfrId && item.ItemNumber == itemNumber);

            Guard.ForNull(itemFromContext, "itemFromContext");

            return InventoryItemToRead.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItem> GetItemEntityAsync(long id)
        {
            return await context.InventoryItems
                                .Include(item => item.Manufacturer)
                                .Include(item => item.ProductCode)
                                    .ThenInclude(pc => pc.SaleCode)
                                .Include(item => item.Part)
                                .Include(item => item.Labor)
                                .Include(item => item.Tire)
                                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync()
        {
            var items = new List<InventoryItemToRead>();

            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(pc => pc.SaleCode)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .AsNoTracking()
                                                .ToArrayAsync();

            foreach (var item in itemsFromContext)
                items.Add(InventoryItemToRead.ConvertToDto(item));

            return items;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync()
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(pc => pc.SaleCode)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => ConvertToDto(item))
                                   .ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync(long mfrId)
        {
            var itemsFromContext = await context.InventoryItems
                                                .Include(item => item.Manufacturer)
                                                .Include(item => item.ProductCode)
                                                    .ThenInclude(pc => pc.SaleCode)
                                                .Include(item => item.Part)
                                                .Include(item => item.Labor)
                                                .Include(item => item.Tire)
                                                .Where(item => item.ManufacturerId == mfrId)
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => ConvertToDto(item))
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

        private static InventoryItemToReadInList ConvertToDto(InventoryItem item)
        {
            if (item == null)
                return null;

            InventoryItemToReadInList itemInList = new();
            itemInList.Id = item.Id;
            itemInList.Manufacturer = item.Manufacturer;
            itemInList.ManufacturerId = item.ManufacturerId;
            itemInList.ManufacturerName = item.Manufacturer?.Name;
            itemInList.ItemNumber = item.ItemNumber;
            itemInList.Description = item.Description;
            itemInList.ProductCode = item.ProductCode;
            itemInList.ProductCodeId = item.ProductCodeId;
            itemInList.ProductCodeName = item.ProductCode?.Name;
            itemInList.ItemType = item.ItemType;

            return itemInList;
        }
    }
}

