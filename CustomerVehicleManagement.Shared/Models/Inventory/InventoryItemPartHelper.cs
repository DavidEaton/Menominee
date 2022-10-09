using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPartHelper
    {
        public static InventoryItemPartToWrite ConvertReadToWriteDto(InventoryItemPartToRead part)
        {
            return part is null
                ? null
                : (new()
                {
                    List = part.List,
                    Cost = part.Cost,
                    Core = part.Core,
                    Retail = part.Retail,
                    TechAmount = new TechAmountToWrite()
                    {
                        PayType = part.TechAmount.PayType,
                        Amount = part.TechAmount.Amount,
                        SkillLevel = part.TechAmount.SkillLevel
                    },
                    LineCode = part.LineCode,
                    SubLineCode = part.SubLineCode,
                    Fractional = part.Fractional
                });
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
                TechAmount.Create(
                    part.TechAmount.PayType,
                    part.TechAmount.Amount,
                    part.TechAmount.SkillLevel)
                .Value,
                part.LineCode,
                part.SubLineCode,
                part.Fractional).Value;
        }

        public static InventoryItemPartToRead ConvertEntityToReadDto(InventoryItemPart part)
        {
            return part is null
                ? null
                : (new()
                {
                    Id = part.Id,
                    List = part.List,
                    Cost = part.Cost,
                    Core = part.Core,
                    Retail = part.Retail,
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = part.TechAmount.PayType,
                        Amount = part.TechAmount.Amount,
                        SkillLevel = part.TechAmount.SkillLevel
                    },
                    LineCode = part.LineCode,
                    SubLineCode = part.SubLineCode,
                    Fractional = part.Fractional
                });
        }

        public static InventoryItemPartToReadInList ConvertEntityToReadInListDto(InventoryItemPart part)
        {
            return part is null
                ? null
                : (new()
                {
                    List = part.List,
                    Cost = part.Cost,
                    Core = part.Core,
                    Retail = part.Retail,
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = part.TechAmount.PayType,
                        Amount = part.TechAmount.Amount,
                        SkillLevel = part.TechAmount.SkillLevel
                    },
                    LineCode = part.LineCode,
                    SubLineCode = part.SubLineCode,
                    Fractional = part.Fractional
                });
        }
    }
}
