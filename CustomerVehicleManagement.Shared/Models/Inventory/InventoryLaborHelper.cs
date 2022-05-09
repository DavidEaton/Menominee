using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryLaborHelper
    {
        public static InventoryLaborToWrite CreateInventoryLabor(InventoryLaborToRead labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                LaborType = labor.LaborType,
                LaborAmount = labor.LaborAmount,
                TechPayType = labor.TechPayType,
                TechPayAmount = labor.TechPayAmount,
                SkillLevel = labor.SkillLevel
            };
        }

        public static InventoryItemLabor CreateInventoryLabor(InventoryLaborToWrite labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                LaborType = labor.LaborType,
                LaborAmount = labor.LaborAmount,
                TechPayType = labor.TechPayType,
                TechPayAmount = labor.TechPayAmount,
                SkillLevel = labor.SkillLevel
            };
        }

        public static InventoryLaborToRead CreateInventoryLabor(InventoryItemLabor labor)
        {
            if (labor is null)
                return null;

            return new()
            {
                Id = labor.Id,
                LaborType = labor.LaborType,
                LaborAmount = labor.LaborAmount,
                TechPayType = labor.TechPayType,
                TechPayAmount = labor.TechPayAmount,
                SkillLevel = labor.SkillLevel
            };
        }

        public static void CopyInventoryLabor(InventoryLaborToWrite laborToUpdate, InventoryItemLabor labor)
        {
            labor.LaborType = laborToUpdate.LaborType;
            labor.LaborAmount = laborToUpdate.LaborAmount;
            labor.TechPayType = laborToUpdate.TechPayType;
            labor.TechPayAmount = laborToUpdate.TechPayAmount;
            labor.SkillLevel = laborToUpdate.SkillLevel;
        }

        public static InventoryLaborToReadInList CreateLaborItemInList(InventoryItemLabor labor)
        {
            if (labor is null)
                return null;

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
