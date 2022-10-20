using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryWarrantyHelper
    {
        public static InventoryItemWarrantyToWrite ConvertReadToWriteDto(InventoryItemWarrantyToRead warranty)
        {
            return warranty is null
                ? null
                : (new()
                {
                    PeriodType =
                    (InventoryItemWarrantyPeriodType)Enum.Parse(
                        typeof(InventoryItemWarrantyPeriodType),
                        warranty.WarrantyPeriod.PeriodType),
                    Duration = warranty.WarrantyPeriod.Duration
                });
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

        public static InventoryItemWarrantyToRead ConvertEntityToReadDto(InventoryItemWarranty warranty)
        {
            return warranty is null
                ? null
                : (new()
                {
                    Id = warranty.Id,
                    WarrantyPeriod = new InventoryItemWarrantyPeriodToRead()
                    {
                        PeriodType = warranty.WarrantyPeriod.PeriodType.ToString(),
                        Duration = warranty.WarrantyPeriod.Duration
                    }
                });
        }

        public static InventoryWarrantyToReadInList ConvertEntityToReadInListDto(InventoryItemWarranty warranty)
        {
            return warranty is null
                ? null
                : (new()
                {
                    PeriodType = warranty.WarrantyPeriod.PeriodType,
                    Duration = warranty.WarrantyPeriod.Duration
                });
        }
    }
}

