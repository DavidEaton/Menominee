using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemRepository
    {
        Task AddInventoryItemAsync(InventoryItem item);
        Task<InventoryItem> GetInventoryItemEntityAsync(long mfrId, string partNumber);
        Task<InventoryItem> GetInventoryItemEntityAsync(long id);
        Task<InventoryItemToRead> GetInventoryItemAsync(long mfrId, string partNumber);
        Task<InventoryItemToRead> GetInventoryItemAsync(long id);
        Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync();
        void UpdateInventoryItemAsync(InventoryItem item);
        Task DeleteInventoryItemAsync(long id);
        Task<bool> InventoryItemExistsAsync(long mfrId, string partNumber);
        Task<bool> InventoryItemExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
