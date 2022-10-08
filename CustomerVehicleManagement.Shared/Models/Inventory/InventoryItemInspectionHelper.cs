using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemInspectionHelper
    {
        public static InventoryItemInspectionToWrite ConvertReadToWriteDto(InventoryItemInspectionToRead inspection)
        {
            if (inspection is null)
                return null;

            return new()
            {
                LaborType = inspection.LaborType,
                LaborAmount = inspection.LaborAmount,
                TechPayType = inspection.TechPayType,
                TechPayAmount = inspection.TechPayAmount,
                SkillLevel = inspection.SkillLevel,
                Type = inspection.Type
            };
        }

        public static InventoryItemInspection ConvertWriteDtoToEntity(InventoryItemInspectionToWrite inspection)
        {
            if (inspection is null)
                return null;

            return InventoryItemInspection.Create(
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
            if (inspection is null)
                return null;

            return new()
            {
                Id = inspection.Id,
                LaborType = inspection.LaborType,
                LaborAmount = inspection.LaborAmount,
                TechPayType = inspection.TechPayType,
                TechPayAmount = inspection.TechPayAmount,
                SkillLevel = inspection.SkillLevel,
                Type = inspection.InspectionType
            };
        }

        public static void CopyWriteDtoToEntity(InventoryItemInspectionToWrite inspectionToUpdate, InventoryItemInspection inspection)
        {
            inspection.LaborType = inspectionToUpdate.LaborType;
            inspection.LaborAmount = inspectionToUpdate.LaborAmount;
            inspection.TechPayType = inspectionToUpdate.TechPayType;
            inspection.TechPayAmount = inspectionToUpdate.TechPayAmount;
            inspection.SkillLevel = inspectionToUpdate.SkillLevel;
            inspection.Type = inspectionToUpdate.Type;
        }

        public static InventoryItemInspectionToReadInList ConvertEntityToReadInListDto(InventoryItemInspection inspection)
        {
            if (inspection is null)
                return null;

            return new()
            {
                LaborType = inspection.LaborType,
                LaborAmount = inspection.LaborAmount,
                TechPayType = inspection.TechPayType,
                TechPayAmount = inspection.TechPayAmount,
                SkillLevel = inspection.SkillLevel,
                Type = inspection.InspectionType
            };
        }
    }
}
