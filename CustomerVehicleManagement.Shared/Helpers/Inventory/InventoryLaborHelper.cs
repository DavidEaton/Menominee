using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Helpers.Inventory
{
    public class InventoryLaborHelper
    {
        public static InventoryItemLabor CreateEntityFromWriteDto(InventoryLaborToWrite labor)
        {
            return new()
            {
                LaborType = labor.LaborType,
                LaborAmount = labor.LaborAmount,
                TechPayType = labor.TechPayType,
                TechPayAmount = labor.TechPayAmount,
                SkillLevel = labor.SkillLevel
            };
        }

        public static InventoryLaborToWrite CreateWriteDtoFromReadDto(InventoryLaborToRead labor)
        {
            return new()
            {
                LaborType = labor.LaborType,
                LaborAmount = labor.LaborAmount,
                TechPayType = labor.TechPayType,
                TechPayAmount = labor.TechPayAmount,
                SkillLevel = labor.SkillLevel
            };
        }

        public static void CopyWriteDtoToEntity(InventoryLaborToWrite laborToUpdate, InventoryItemLabor labor)
        {
            labor.LaborType = laborToUpdate.LaborType;
            labor.LaborAmount = laborToUpdate.LaborAmount;
            labor.TechPayType = laborToUpdate.TechPayType;
            labor.TechPayAmount = laborToUpdate.TechPayAmount;
            labor.SkillLevel = laborToUpdate.SkillLevel;
        }
    }
}
