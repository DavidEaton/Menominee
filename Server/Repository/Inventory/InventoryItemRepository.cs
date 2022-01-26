using MenomineePlayWASM.Shared;
using MenomineePlayWASM.Shared.Dtos.Inventory;
using MenomineePlayWASM.Shared.Entities.Inventory;
using MenomineePlayWASM.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Inventory
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
            if (item != null)
                await context.AddAsync(item);
        }

        public async Task DeleteItemAsync(long id)
        {
            var itemFromContext = await context.InventoryItems.FindAsync(id);
            if (itemFromContext != null)
                context.Remove(itemFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<InventoryItemToRead> GetItemAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                                               .Include(item => item.Manufacturer)
                                               .FirstOrDefaultAsync(item => item.Id == id);

            return new InventoryItemToRead()
            {
                //Manufacturer = new Manufacturer()
                //{
                //},
                Id = itemFromContext.Id,
                PartNumber = itemFromContext.PartNumber,
                Description = itemFromContext.Description,
                PartType = itemFromContext.PartType.ToString(),
                Retail = itemFromContext.Retail,
                Cost = itemFromContext.Cost,
                Core = itemFromContext.Core,
                Labor = itemFromContext.Labor,
                OnHand = itemFromContext.OnHand
            };
        }

        public async Task<InventoryItem> GetItemEntityAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                                           .Include(item => item.Manufacturer)
                                           .FirstOrDefaultAsync(item => item.Id == id);

            return itemFromContext;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetItemListAsync()
        {
            IReadOnlyList<InventoryItem> items = await context.InventoryItems
                                                              .Include(item => item.Manufacturer)
                                                              .ToListAsync();

            return items
                .Select(item => new InventoryItemToReadInList
                {
                    // manufacturer
                    Id = item.Id,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    PartType = item.PartType.ToString(),
                    Retail = item.Retail,
                    Cost = item.Cost,
                    Core = item.Core,
                    Labor = item.Labor,
                    OnHand = item.OnHand
                }).ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync()
        {
            IReadOnlyList<InventoryItem> itemsFromContext = await context.InventoryItems.ToListAsync();

            return itemsFromContext
                .Select(item => new InventoryItemToRead()
                {
                    // manufacturer
                    Id = item.Id,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    PartType = item.PartType.ToString(),
                    Retail = item.Retail,
                    Cost = item.Cost,
                    Core = item.Core,
                    Labor = item.Labor,
                    OnHand = item.OnHand
                }).ToList();
        }

        public async Task<bool> ItemExistsAsync(long id)
        {
            return await context.InventoryItems.AnyAsync(item => item.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateItemAsync(InventoryItem item)
        {
            // No code in this implementation
        }
    }
}
