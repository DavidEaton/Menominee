using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemPartRepositoryXXX
    {
        Task AddPartAsync(InventoryItemPart part);
        Task DeletePartAsync(long id);
        void FixTrackingState();
        Task<bool> PartExistsAsync(long id);
        Task<InventoryItemPart> UpdatePartAsync(InventoryItemPart part);
        Task<IReadOnlyList<InventoryPartToRead>> GetPartsAsync();
        Task<IReadOnlyList<InventoryPartToReadInList>> GetPartsInListAsync();
        Task<InventoryPartToRead> GetPartAsync(long id);
        Task<InventoryItemPart> GetPartEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
