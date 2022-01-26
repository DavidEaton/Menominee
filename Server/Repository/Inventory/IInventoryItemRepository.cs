using MenomineePlayWASM.Shared.Dtos.Inventory;
using MenomineePlayWASM.Shared.Entities.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Inventory
{
    public interface IInventoryItemRepository
    {
        Task AddItemAsync(InventoryItem item);
        Task<InventoryItem> GetItemEntityAsync(long id);
        Task<InventoryItemToRead> GetItemAsync(long id);
        Task<IReadOnlyList<InventoryItemToRead>> GetItemsAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemListAsync();
        void UpdateItemAsync(InventoryItem item);
        Task DeleteItemAsync(long id);
        Task<bool> ItemExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
