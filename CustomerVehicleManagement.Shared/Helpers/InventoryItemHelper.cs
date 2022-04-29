using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList ConvertToReadInListDto(InventoryItem item)
        {
            if (item is not null)
            {
                return new InventoryItemToReadInList
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    PartType = item.PartType,
                    QuantityOnHand = item.QuantityOnHand,
                    Cost = item.Cost,
                    SuggestedPrice = item.SuggestedPrice,
                    Labor = item.Labor
                };
            }

            return null;
        }

        public static InventoryItemToRead ConvertToDto(InventoryItem item)
        {
            if (item != null)
            {
                return new InventoryItemToRead
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    PartType = item.PartType,
                    QuantityOnHand = item.QuantityOnHand,
                    Cost = item.Cost,
                    SuggestedPrice = item.SuggestedPrice,
                    Labor = item.Labor
                };
            }

            return null;
        }

    }
}
