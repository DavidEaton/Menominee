using CustomerVehicleManagement.Shared.Models.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Inventory
{
    public interface IMaintenanceItemDataService
    {
        Task<IReadOnlyList<MaintenanceItemToReadInList>> GetAllItemsAsync();
        Task<MaintenanceItemToRead> GetItemAsync(long id);
        Task<MaintenanceItemToRead> AddItemAsync(MaintenanceItemToWrite item);
        Task UpdateItemAsync(MaintenanceItemToWrite item, long id);
        Task DeleteItemAsync(long id);
    }
}
