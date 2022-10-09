using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemInspectionHelper
    {
        public static InventoryItemInspectionToWrite ConvertReadToWriteDto(InventoryItemInspectionToRead inspection)
        {
            return inspection is null
                ? null
                : (new()
                {
                    LaborAmount = new LaborAmountToWrite()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.PayType,
                    },
                    TechAmount = new TechAmountToWrite()
                    {
                        PayType = inspection.TechAmount.PayType,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.Type
                });
        }

        public static InventoryItemInspection ConvertWriteDtoToEntity(InventoryItemInspectionToWrite inspection)
        {
            return inspection is null
                ? null
                : InventoryItemInspection.Create(
                LaborAmount.Create(
                    inspection.LaborAmount.PayType,
                    inspection.LaborAmount.Amount).
                Value,
                TechAmount.Create(
                    inspection.TechAmount.PayType,
                    inspection.TechAmount.Amount,
                    inspection.TechAmount.SkillLevel)
                .Value,
                inspection.Type).Value;
        }

        public static InventoryItemInspectionToRead ConvertEntityToReadDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : (new()
                {
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.PayType,
                    },
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = inspection.TechAmount.PayType,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.InspectionType,
                });
        }

        public static InventoryItemInspectionToReadInList ConvertEntityToReadInListDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : (new()
                {
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = inspection.LaborAmount.Amount,
                        PayType = inspection.LaborAmount.PayType,
                    },
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = inspection.TechAmount.PayType,
                        Amount = inspection.TechAmount.Amount,
                        SkillLevel = inspection.TechAmount.SkillLevel
                    },

                    Type = inspection.InspectionType,
                });
        }
    }
}
