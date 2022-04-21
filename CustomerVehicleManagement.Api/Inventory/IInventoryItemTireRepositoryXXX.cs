using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemTireRepositoryXXX
    {
        Task AddTireAsync(InventoryItemTire tire);
        Task DeleteTireAsync(long id);
        void FixTrackingState();
        Task<bool> TireExistsAsync(long id);
        Task<InventoryItemTire> UpdateTireAsync(InventoryItemTire tire);
        Task<IReadOnlyList<InventoryTireToRead>> GetTiresAsync();
        Task<IReadOnlyList<InventoryTireToReadInList>> GetTiresInListAsync();
        Task<InventoryTireToRead> GetTireAsync(long id);
        Task<InventoryItemTire> GetTireEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
