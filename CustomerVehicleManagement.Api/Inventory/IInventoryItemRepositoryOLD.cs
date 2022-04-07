using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemRepositoryOLD
    {
        Task AddInventoryItemAsync(InventoryItem item);
        Task<InventoryItem> GetInventoryItemEntityAsync(long mfrId, string partNumber);
        Task<InventoryItem> GetInventoryItemEntityAsync(long id);
        Task<InventoryItemToRead> GetInventoryItemAsync(long mfrId, string partNumber);
        Task<InventoryItemToRead> GetInventoryItemAsync(long id);
        Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync(long mfrId);
        void UpdateInventoryItemAsync(InventoryItem item);
        Task DeleteInventoryItemAsync(long id);
        Task<bool> InventoryItemExistsAsync(long mfrId, string partNumber);
        Task<bool> InventoryItemExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
