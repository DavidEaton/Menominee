using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryInspectionHelper
    {
        public static InventoryInspectionToWrite ConvertReadToWriteDto(InventoryInspectionToRead inspection)
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

        public static InventoryItemInspection ConvertWriteDtoToEntity(InventoryInspectionToWrite inspection)
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

        public static InventoryInspectionToRead ConvertEntityToReadDto(InventoryItemInspection inspection)
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

        public static void CopyWriteDtoToEntity(InventoryInspectionToWrite inspectionToUpdate, InventoryItemInspection inspection)
        {
            inspection.LaborType = inspectionToUpdate.LaborType;
            inspection.LaborAmount = inspectionToUpdate.LaborAmount;
            inspection.TechPayType = inspectionToUpdate.TechPayType;
            inspection.TechPayAmount = inspectionToUpdate.TechPayAmount;
            inspection.SkillLevel = inspectionToUpdate.SkillLevel;
            inspection.Type = inspectionToUpdate.Type;
        }

        public static InventoryInspectionToReadInList ConvertEntityToReadInListDto(InventoryItemInspection inspection)
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
