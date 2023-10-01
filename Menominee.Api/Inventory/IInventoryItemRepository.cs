using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
{
    public interface IInventoryItemRepository
    {
        void Add(InventoryItem entity);
        void Delete(InventoryItem entity);
        Task<InventoryItemToRead> GetAsync(long id);
        Task<InventoryItemToRead> GetAsync(long manufacturerId, string itemNumber);
        Task<IReadOnlyList<InventoryItemToRead>> GetAllAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetListAsync();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetListAsync(long manufacturerId);
        Task<IReadOnlyList<InventoryItem>> GetEntitiesAsync(List<long> ids);
        Task<InventoryItem> GetEntityAsync(long id);
        Task<InventoryItemPart> GetPartEntityAsync(long id);
        Task<InventoryItemWarranty> GetWarrantyEntityAsync(long id);
        Task<InventoryItemInspection> GetInspectionEntityAsync(long id);
        Task<InventoryItemLabor> GetLaborEntityAsync(long id);
        Task<InventoryItemTire> GetTireEntityAsync(long id);
        Task<InventoryItemPackage> GetPackageEntityAsync(long id);
        Task SaveChangesAsync();
    }
}
