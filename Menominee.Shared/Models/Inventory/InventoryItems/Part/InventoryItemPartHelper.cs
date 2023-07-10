using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Part
{
    public class InventoryItemPartHelper
    {
        public static InventoryItemPartToWrite ConvertReadToWriteDto(InventoryItemPartToRead part)
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
                part.Fractional,
                part.LineCode,
                part.SubLineCode)
            .Value;
        }

        public static InventoryItemPartToRead ConvertToReadDto(InventoryItemPart part)
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

        public static InventoryItemPartToReadInList ConvertToReadInListDto(InventoryItemPart part)
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

        public static InventoryItemPartToWrite ConvertToWriteDto(InventoryItemPart part)
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
    }
}
