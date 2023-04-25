using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire
{
    public class InventoryItemTireHelper
    {
        public static InventoryItemTireToWrite ConvertReadToWriteDto(InventoryItemTireToRead tire)
        {
            return tire is null
                ? new()
                : (new()
                {
                    List = tire.List,
                    Cost = tire.Cost,
                    Core = tire.Core,
                    Retail = tire.Retail,
                    TechAmount = new TechAmountToWrite()
                    {
                        Amount = tire.TechAmount.Amount,
                        PayType = tire.TechAmount.PayType,
                        SkillLevel = tire.TechAmount.SkillLevel
                    },
                    LineCode = tire.LineCode,
                    SubLineCode = tire.SubLineCode,
                    Fractional = tire.Fractional,
                    Type = tire.Type,
                    Width = tire.Width,
                    AspectRatio = tire.AspectRatio,
                    Diameter = tire.Diameter,
                    LoadIndex = tire.LoadIndex,
                    SpeedRating = tire.SpeedRating
                });
        }

        public static InventoryItemTire ConvertWriteDtoToEntity(InventoryItemTireToWrite tire)
        {
            return tire is null
                ? null
                : InventoryItemTire.Create(
                tire.Width,
                tire.AspectRatio,
                tire.ConstructionType,
                tire.Diameter,
                tire.List,
                tire.Cost,
                tire.Core,
                tire.Retail,
                TechAmount.Create(
                    tire.TechAmount.PayType,
                    tire.TechAmount.Amount,
                    tire.TechAmount.SkillLevel)
                .Value,
                tire.Fractional,
                tire.LineCode,
                tire.SubLineCode,
                tire.Type,
                tire.LoadIndex,
                tire.SpeedRating)
            .Value;
        }

        public static InventoryItemTireToRead ConvertEntityToReadDto(InventoryItemTire tire)
        {
            return tire is null
                ? new()
                : new()
                {
                    Id = tire.Id,
                    List = tire.List,
                    Cost = tire.Cost,
                    Core = tire.Core,
                    Retail = tire.Retail,
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = tire.TechAmount.PayType,
                        Amount = tire.TechAmount.Amount,
                        SkillLevel = tire.TechAmount.SkillLevel
                    },
                    LineCode = tire.LineCode,
                    SubLineCode = tire.SubLineCode,
                    Fractional = tire.Fractional,
                    Type = tire.Type,
                    Width = tire.Width,
                    AspectRatio = tire.AspectRatio,
                    Diameter = tire.Diameter,
                    LoadIndex = tire.LoadIndex,
                    SpeedRating = tire.SpeedRating
                };
        }

        public static InventoryItemTireToReadInList ConvertEntityToReadInListDto(InventoryItemTire tire)
        {
            return tire is null
                ? new()
                : new()
                {
                    List = tire.List,
                    Cost = tire.Cost,
                    Core = tire.Core,
                    Retail = tire.Retail,
                    TechAmount = new TechAmountToRead()
                    {
                        PayType = tire.TechAmount.PayType,
                        Amount = tire.TechAmount.Amount,
                        SkillLevel = tire.TechAmount.SkillLevel
                    },
                    LineCode = tire.LineCode,
                    SubLineCode = tire.SubLineCode,
                    Fractional = tire.Fractional,
                    Type = tire.Type,
                    Width = tire.Width,
                    AspectRatio = tire.AspectRatio,
                    Diameter = tire.Diameter,
                    LoadIndex = tire.LoadIndex,
                    SpeedRating = tire.SpeedRating
                };
        }

        public static InventoryItemTireToWrite ConvertToWriteDto(InventoryItemTire tire)
        {
            return tire is null
                ? new()
                : (new()
                {
                    List = tire.List,
                    Cost = tire.Cost,
                    Core = tire.Core,
                    Retail = tire.Retail,
                    TechAmount = new TechAmountToWrite()
                    {
                        Amount = tire.TechAmount.Amount,
                        PayType = tire.TechAmount.PayType,
                        SkillLevel = tire.TechAmount.SkillLevel
                    },
                    LineCode = tire.LineCode,
                    SubLineCode = tire.SubLineCode,
                    Fractional = tire.Fractional,
                    Type = tire.Type,
                    Width = tire.Width,
                    AspectRatio = tire.AspectRatio,
                    Diameter = tire.Diameter,
                    LoadIndex = tire.LoadIndex,
                    SpeedRating = tire.SpeedRating
                });
        }
    }
}
