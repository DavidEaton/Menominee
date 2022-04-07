using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToReadInList
    {
        public long Id { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public InventoryItemType ItemType { get; set; }
        public long DetailId { get; set; }

        //public int QuantityOnHand { get; set; }
        //public double Cost { get; set; }
        //public double SuggestedPrice { get; set; }
        //public double Labor { get; set; }

        public static InventoryItemToReadInList ConvertToDto(InventoryItem item)
        {
            if (item != null)
            {
                return new InventoryItemToReadInList
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    ItemNumber = item.ItemNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    ItemType = item.ItemType,
                    DetailId = item.DetailId
                    //QuantityOnHand = item.QuantityOnHand,
                    //Cost = item.Cost,
                    //SuggestedPrice = item.SuggestedPrice,
                    //Labor = item.Labor
                };
            }

            return null;
        }
    }
}
