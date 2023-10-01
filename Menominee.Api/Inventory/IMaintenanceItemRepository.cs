using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
{
    public interface IMaintenanceItemRepository
    {
        void Add(MaintenanceItem entity);
        void Delete(MaintenanceItem entity);
        Task<IReadOnlyList<MaintenanceItemToReadInList>> GetListAsync();
        Task<MaintenanceItemToRead> GetAsync(long id);
        Task<MaintenanceItem> GetEntityAsync(long id);
        Task SaveChangesAsync();
    }
}
