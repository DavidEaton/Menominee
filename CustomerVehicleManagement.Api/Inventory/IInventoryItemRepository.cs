using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemRepository
    {
        Task AddItemAsync(InventoryItem item);
        Task DeleteItemAsync(long id);
        void FixTrackingState();
        Task<bool> ItemExistsAsync(long id);
        Task<InventoryItem> UpdateItemAsync(InventoryItem item);
        Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync(long mfrId);
        Task<InventoryItemToRead> GetItemAsync(long id);
        Task<InventoryItemToRead> GetItemAsync(long mfrId, string itemNumber);
        Task<InventoryItem> GetItemEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
