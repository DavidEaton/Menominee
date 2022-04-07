using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryPartRepository
    {
        Task AddPartAsync(InventoryPart part);
        Task DeletePartAsync(long id);
        void FixTrackingState();
        Task<bool> PartExistsAsync(long id);
        Task<InventoryItem> UpdatePartAsync(InventoryPart part);
        Task<IReadOnlyList<InventoryPartToRead>> GetPartsAsync();
        Task<IReadOnlyList<InventoryPartToReadInList>> GetPartsInListAsync();
        Task<InventoryPartToRead> GetPartAsync(long id);
        Task<InventoryPart> GetPartEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
