using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Inventory
{
    public interface IInventoryItemDataService
    {
        Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItems();
        Task<InventoryItemToRead> GetItem(long id);
        Task<InventoryItemToRead> AddItem(InventoryItemToWrite item);
        Task UpdateItem(InventoryItemToWrite item, long id);
    }
}
