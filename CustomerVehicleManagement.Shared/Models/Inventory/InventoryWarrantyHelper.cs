using CustomerVehicleManagement.Domain.Entities.Inventory;

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
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
        }

        public static InventoryItemWarranty ConvertWriteDtoToEntity(InventoryItemWarrantyToWrite warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
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

