using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToRead
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
        public InventoryPartToRead Part { get; set; }
        public InventoryLaborToRead Labor { get; set; }
        public InventoryTireToRead Tire { get; set; }


        //public int QuantityOnHand { get; set; }
        //public double Cost { get; set; }
        //public double SuggestedPrice { get; set; }
        //public double Labor { get; set; }

        public static InventoryItemToRead ConvertToDto(InventoryItem item)
        {
            if (item != null)
            {
                var itemReadDto = new InventoryItemToRead
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
                };

                if (item.ItemType == InventoryItemType.Part)
                {
                    itemReadDto.Part = InventoryPartToRead.ConvertToDto(item.Part);
                }
                else if (item.ItemType == InventoryItemType.Labor)
                {
                    itemReadDto.Labor = InventoryLaborToRead.ConvertToDto(item.Labor);
                }
                else if (item.ItemType == InventoryItemType.Tire)
                {
                    itemReadDto.Tire = InventoryTireToRead.ConvertToDto(item.Tire);
                }

                return itemReadDto;
            }

            return null;
        }
    }
}
