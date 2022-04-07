using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Helpers.Inventory
{
    public class InventoryPartHelper
    {
        public static InventoryPart CreateEntityFromWriteDto(InventoryPartToWrite part)
        {
            return new()
            {
                List = part.List,
                Cost = part.Cost,
                Core = part.Core,
                Retail = part.Retail,
                TechPayType = part.TechPayType,
                TechPayAmount = part.TechPayAmount,
                LineCode = part.LineCode,
                SubLineCode = part.SubLineCode,
                Fractional = part.Fractional,
                SkillLevel = part.SkillLevel
            };
        }

        public static InventoryPartToWrite CreateWriteDtoFromReadDto(InventoryPartToRead part)
        {
            return new()
            {
                List = part.List,
                Cost = part.Cost,
                Core = part.Core,
                Retail = part.Retail,
                TechPayType = part.TechPayType,
                TechPayAmount = part.TechPayAmount,
                LineCode = part.LineCode,
                SubLineCode = part.SubLineCode,
                Fractional = part.Fractional,
                SkillLevel = part.SkillLevel
            };
        }
    }
}
