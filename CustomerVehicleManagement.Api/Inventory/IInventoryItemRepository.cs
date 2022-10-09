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
        Task<bool> ItemExistsAsync(long id);
        Task<InventoryItem> UpdateItemAsync(InventoryItem item);
        Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInListAsync(long manufacturerId);
        Task<InventoryItemToRead> GetItemAsync(long id);
        Task<InventoryItemToRead> GetItemAsync(long manufacturerId, string itemNumber);
        Task<InventoryItem> GetItemEntityAsync(long id);
        Task<InventoryItemPart> GetInventoryItemPartEntityAsync(long id);
        Task<InventoryItemWarranty> GetInventoryItemWarrantyEntityAsync(long id);
        Task<InventoryItemInspection> GetInventoryItemInspectionEntityAsync(long id);
        Task<InventoryItemLabor> GetInventoryItemLaborEntityAsync(long id);
        Task<InventoryItemTire> GetInventoryItemTireEntityAsync(long id);
        Task<InventoryItemPackage> GetInventoryItemPackageEntityAsync(long id);
        Task<IReadOnlyList<InventoryItem>> GetInventoryItemEntitiesAsync(List<long> ids);
        Task SaveChangesAsync();
        void DeleteInventoryItem(InventoryItem entity);
    }
}
