using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
{
    public interface IMaintenanceItemRepository
    {
        Task AddItemAsync(MaintenanceItem item);
        Task DeleteItemAsync(long id);
        Task<bool> ItemExistsAsync(long id);
        Task<MaintenanceItem> UpdateItemAsync(MaintenanceItem item);
        Task<IReadOnlyList<MaintenanceItemToReadInList>> GetItemsInListAsync();
        Task<MaintenanceItemToRead> GetItemAsync(long id);
        Task<MaintenanceItem> GetItemEntityAsync(long id);
        Task<bool> SaveChangesAsync();
        void DeleteItem(MaintenanceItem entity);
    }
}
