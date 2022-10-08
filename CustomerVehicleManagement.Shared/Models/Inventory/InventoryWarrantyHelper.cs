using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryWarrantyHelper
    {
        public static InventoryItemWarrantyToWrite ConvertReadToWriteDto(InventoryItemWarrantyToRead warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                PeriodType = 
                    (InventoryItemWarrantyPeriodType)Enum.Parse(
                        typeof(InventoryItemWarrantyPeriodType),
                        warranty.WarrantyPeriod.PeriodType),
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

        public static InventoryItemWarrantyToRead ConvertEntityToReadDto(InventoryItemWarranty warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                Id = warranty.Id,
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
        }

        public static void CopyWriteDtoToEntity(InventoryItemWarrantyToWrite warrantyToUpdate, InventoryItemWarranty warranty)
        {
            warranty.PeriodType = warrantyToUpdate.PeriodType;
            warranty.Duration = warrantyToUpdate.Duration;
        }

        public static InventoryWarrantyToReadInList ConvertEntityToReadInListDto(InventoryItemWarranty warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
        }
    }
}

