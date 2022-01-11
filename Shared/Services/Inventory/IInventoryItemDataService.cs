using MenomineePlayWASM.Shared.Dtos.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Services.Inventory
{
    public interface IInventoryItemDataService
    {
        Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItems();
        Task<InventoryItemToRead> GetItem(long id);
        Task<InventoryItemToRead> AddItem(InventoryItemToWrite item);
        Task UpdateItem(InventoryItemToWrite item, long id);
    }
}
