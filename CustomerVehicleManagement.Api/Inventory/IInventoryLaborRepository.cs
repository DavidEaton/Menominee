using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryLaborRepository
    {
        Task AddLaborAsync(InventoryLabor labor);
        Task DeleteLaborAsync(long id);
        void FixTrackingState();
        Task<bool> LaborExistsAsync(long id);
        Task<InventoryLabor> UpdateLaborAsync(InventoryLabor labor);
        Task<IReadOnlyList<InventoryLaborToRead>> GetLaborsAsync();
        Task<IReadOnlyList<InventoryLaborToReadInList>> GetLaborListAsync();
        Task<InventoryLaborToRead> GetLaborAsync(long id);
        Task<InventoryLabor> GetLaborEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
