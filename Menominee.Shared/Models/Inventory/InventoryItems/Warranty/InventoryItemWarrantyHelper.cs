using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryItemWarrantyHelper
    {
        public static InventoryItemWarrantyToWrite ConvertReadToWriteDto(InventoryItemWarrantyToRead warranty)
        {
            return warranty is null
                ? null
                : new()
                {
                    PeriodType = warranty.WarrantyPeriod.PeriodType,
                    Duration = warranty.WarrantyPeriod.Duration
                };
        }

        public static InventoryItemWarranty ConvertWriteDtoToEntity(InventoryItemWarrantyToWrite warranty)
        {
            return warranty is null
                ? null
                : InventoryItemWarranty.Create(
                    InventoryItemWarrantyPeriod.Create(
                        warranty.PeriodType,
                        warranty.Duration)
                .Value)
            .Value;
        }

        public static InventoryItemWarrantyToRead ConvertToReadDto(InventoryItemWarranty warranty)
        {
            return warranty is null
                ? null
                : new()
                {
                    Id = warranty.Id,
                    WarrantyPeriod = new InventoryItemWarrantyPeriodToRead()
                    {
                        PeriodType = warranty.WarrantyPeriod.PeriodType,
                        Duration = warranty.WarrantyPeriod.Duration
                    }
                };
        }

        public static InventoryItemWarrantyToReadInList ConvertToReadInListDto(InventoryItemWarranty warranty)
        {
            return warranty is null
                ? null
                : new()
                {
                    PeriodType = warranty.WarrantyPeriod.PeriodType,
                    Duration = warranty.WarrantyPeriod.Duration
                };
        }

        public static InventoryItemWarrantyToWrite ConvertToWriteDto(InventoryItemWarranty warranty)
        {
            return warranty is null
                ? null
                : new()
                {
                    Id = warranty.Id,
                    PeriodType = warranty.WarrantyPeriod.PeriodType,
                    Duration = warranty.WarrantyPeriod.Duration
                };
        }
    }
}

