using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IMaintenanceItemRepository
    {
        Task AddItemAsync(MaintenanceItem item);
        Task DeleteItemAsync(long id);
        void FixTrackingState();
        Task<bool> ItemExistsAsync(long id);
        Task<MaintenanceItem> UpdateItemAsync(MaintenanceItem item);
        Task<IReadOnlyList<MaintenanceItemToReadInList>> GetItemsInListAsync();
        Task<MaintenanceItemToRead> GetItemAsync(long id);
        Task<MaintenanceItem> GetItemEntityAsync(long id);
        Task<bool> SaveChangesAsync();
        void DeleteItem(MaintenanceItem entity);
    }
}
