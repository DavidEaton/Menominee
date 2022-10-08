using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemLaborHelper
    {
        public static InventoryItemLaborToWrite ConvertReadToWriteDto(InventoryItemLaborToRead labor)
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

        public static InventoryItemLabor ConvertWriteDtoToEntity(InventoryItemLaborToWrite labor)
        {
            return labor is null
                ? null
                : InventoryItemLabor.Create(
                LaborAmount.Create(labor.LaborAmount.PayType, labor.LaborAmount.Amount).Value,
                TechAmount.Create(labor.TechPayAmount.PayType, labor.TechPayAmount.Amount, labor.TechPayAmount.SkillLevel).Value)
            .Value;
        }

        public static InventoryItemLaborToRead ConvertEntityToReadDto(InventoryItemLabor labor)
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

        public static void CopyWriteDtoToEntity(InventoryItemLaborToWrite laborToUpdate, InventoryItemLabor labor)
        {
            labor.LaborType = laborToUpdate.LaborType;
            labor.LaborAmount = laborToUpdate.LaborAmount;
            labor.TechPayType = laborToUpdate.TechPayType;
            labor.TechPayAmount = laborToUpdate.TechPayAmount;
            labor.SkillLevel = laborToUpdate.SkillLevel;
        }

        public static InventoryItemLaborToReadInList ConvertEntityToReadInListDto(InventoryItemLabor labor)
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
