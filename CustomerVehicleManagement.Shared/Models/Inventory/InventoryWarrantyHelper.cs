using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryWarrantyHelper
    {
        public static InventoryWarrantyToWrite ConvertReadToWriteDto(InventoryWarrantyToRead warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
        }

        public static InventoryItemWarranty ConvertWriteDtoToEntity(InventoryWarrantyToWrite warranty)
        {
            if (warranty is null)
                return null;

            return new()
            {
                PeriodType = warranty.PeriodType,
                Duration = warranty.Duration
            };
        }

        public static InventoryWarrantyToRead ConvertEntityToReadDto(InventoryItemWarranty warranty)
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

        public static void CopyWriteDtoToEntity(InventoryWarrantyToWrite warrantyToUpdate, InventoryItemWarranty warranty)
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

