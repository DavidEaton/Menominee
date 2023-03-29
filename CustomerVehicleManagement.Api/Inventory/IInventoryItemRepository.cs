using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public interface IInventoryItemRepository
    {
        Task Add(InventoryItem entity);
        Task<bool> Exists(long id);
        Task<InventoryItemToRead> GetItem(long id);
        Task<InventoryItemToRead> GetItem(long manufacturerId, string itemNumber);
        Task<IReadOnlyList<InventoryItemToRead>> GetItems();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInList();
        Task<IReadOnlyList<InventoryItemToReadInList>> GetItemsInList(long manufacturerId);
        Task<IReadOnlyList<InventoryItem>> GetInventoryItemEntities(List<long> ids);
        Task<InventoryItem> GetItemEntity(long id);
        Task<InventoryItemPart> GetInventoryItemPartEntity(long id);
        Task<InventoryItemWarranty> GetInventoryItemWarrantyEntity(long id);
        Task<InventoryItemInspection> GetInventoryItemInspectionEntity(long id);
        Task<InventoryItemLabor> GetInventoryItemLaborEntity(long id);
        Task<InventoryItemTire> GetInventoryItemTireEntity(long id);
        Task<InventoryItemPackage> GetInventoryItemPackageEntity(long id);
        Task SaveChanges();
        void Delete(InventoryItem entity);
    }
}
