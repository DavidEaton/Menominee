using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryTireRepository
    {
        Task AddTireAsync(InventoryTire tire);
        Task DeleteTireAsync(long id);
        void FixTrackingState();
        Task<bool> TireExistsAsync(long id);
        Task<InventoryTire> UpdateTireAsync(InventoryTire tire);
        Task<IReadOnlyList<InventoryTireToRead>> GetTiresAsync();
        Task<IReadOnlyList<InventoryTireToReadInList>> GetTiresInListAsync();
        Task<InventoryTireToRead> GetTireAsync(long id);
        Task<InventoryTire> GetTireEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
