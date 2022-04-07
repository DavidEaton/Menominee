using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Helpers.Inventory
{
    public class InventoryLaborHelper
    {
        public static InventoryLabor CreateEntityFromWriteDto(InventoryLaborToWrite labor)
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
    }
}
