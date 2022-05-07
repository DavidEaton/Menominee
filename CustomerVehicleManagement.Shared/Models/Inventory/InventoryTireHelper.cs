using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryTireHelper
    {
        public static InventoryTireToWrite Transform(InventoryTireToRead tire)
        {
            return new()
            {
                List = tire.List,
                Cost = tire.Cost,
                Core = tire.Core,
                Retail = tire.Retail,
                TechPayType = tire.TechPayType,
                TechPayAmount = tire.TechPayAmount,
                LineCode = tire.LineCode,
                SubLineCode = tire.SubLineCode,
                Fractional = tire.Fractional,
                SkillLevel = tire.SkillLevel,
                Type = tire.Type,
                Width = tire.Width,
                AspectRatio = tire.AspectRatio,
                Diameter = tire.Diameter,
                LoadIndex = tire.LoadIndex,
                SpeedRating = tire.SpeedRating
            };
        }

        public static InventoryItemTire Transform(InventoryTireToWrite tire)
        {
            return new()
            {
                List = tire.List,
                Cost = tire.Cost,
                Core = tire.Core,
                Retail = tire.Retail,
                TechPayType = tire.TechPayType,
                TechPayAmount = tire.TechPayAmount,
                LineCode = tire.LineCode,
                SubLineCode = tire.SubLineCode,
                Fractional = tire.Fractional,
                SkillLevel = tire.SkillLevel,
                Type = tire.Type,
                Width = tire.Width,
                AspectRatio = tire.AspectRatio,
                Diameter = tire.Diameter,
                LoadIndex = tire.LoadIndex,
                SpeedRating = tire.SpeedRating
            };
        }

        public static InventoryTireToRead Transform(InventoryItemTire tire)
        {
            return new()
            {
                Id = tire.Id,
                List = tire.List,
                Cost = tire.Cost,
                Core = tire.Core,
                Retail = tire.Retail,
                TechPayType = tire.TechPayType,
                TechPayAmount = tire.TechPayAmount,
                LineCode = tire.LineCode,
                SubLineCode = tire.SubLineCode,
                Fractional = tire.Fractional,
                SkillLevel = tire.SkillLevel,
                Type = tire.Type,
                Width = tire.Width,
                AspectRatio = tire.AspectRatio,
                Diameter = tire.Diameter,
                LoadIndex = tire.LoadIndex,
                SpeedRating = tire.SpeedRating
            };
        }

        public static void CopyWriteDtoToEntity(InventoryTireToWrite tireToUpdate, InventoryItemTire tire)
        {
            tire.List = tireToUpdate.List;
            tire.Cost = tireToUpdate.Cost;
            tire.Core = tireToUpdate.Core;
            tire.Retail = tireToUpdate.Retail;
            tire.TechPayType = tireToUpdate.TechPayType;
            tire.TechPayAmount = tireToUpdate.TechPayAmount;
            tire.LineCode = tireToUpdate.LineCode;
            tire.SubLineCode = tireToUpdate.SubLineCode;
            tire.Fractional = tireToUpdate.Fractional;
            tire.SkillLevel = tireToUpdate.SkillLevel;
            tire.Type = tireToUpdate.Type;
            tire.Width = tireToUpdate.Width;
            tire.AspectRatio = tireToUpdate.AspectRatio;
            tire.Diameter = tireToUpdate.Diameter;
            tire.LoadIndex = tireToUpdate.LoadIndex;
            tire.SpeedRating = tireToUpdate.SpeedRating;
        }
    }
}
