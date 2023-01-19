using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Inventory
{
    public interface IInventoryItemDataService
    {
        Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItemsAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItemsAsync(long mfrId);
        Task<InventoryItemToRead> GetItemAsync(long id);
        Task<InventoryItemToRead> AddItemAsync(InventoryItemToWrite item);
        Task UpdateItemAsync(InventoryItemToWrite item, long id);
    }
}
