using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPartHelper
    {
        public static InventoryItemPartToWrite ConvertReadToWriteDto(InventoryItemPartToRead part)
        {
            if (part is null)
                return null;

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

        public static InventoryItemPart ConvertWriteDtoToEntity(InventoryItemPartToWrite part)
        {
            return part is null
                ? null
                : InventoryItemPart.Create(
                part.List,
                part.Cost,
                part.Core,
                part.Retail,
                TechAmount.Create(part.TechAmount.PayType, part.TechAmount.Amount, part.TechAmount.SkillLevel).Value,
                part.LineCode,
                part.SubLineCode,
                part.Fractional).Value;
        }

        public static InventoryItemPartToRead ConvertEntityToReadDto(InventoryItemPart part)
        {
            if (part is null)
                return null;

            return new()
            {
                Id = part.Id,
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

        public static void CopyWriteDtoToEntity(InventoryItemPartToWrite partToUpdate, InventoryItemPart part)
        {
            part.List = partToUpdate.List;
            part.Cost = partToUpdate.Cost;
            part.Core = partToUpdate.Core;
            part.Retail = partToUpdate.Retail;
            part.TechPayType = partToUpdate.TechPayType;
            part.TechPayAmount = partToUpdate.TechPayAmount;
            part.LineCode = partToUpdate.LineCode;
            part.SubLineCode = partToUpdate.SubLineCode;
            part.Fractional = partToUpdate.Fractional;
            part.SkillLevel = partToUpdate.SkillLevel;
        }

        public static InventoryItemPartToReadInList ConvertEntityToReadInListDto(InventoryItemPart part)
        {
            if (part is null)
                return null;

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
