using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPartHelper
    {
        public static InventoryPartToWrite Transform(InventoryPartToRead part)
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

        public static InventoryItemPart Transform(InventoryPartToWrite part)
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


        //public static InventoryPartToWrite Transform(InventoryPart part)
        //{
        //    return new()
        //    {
        //        List = part.List,
        //        Cost = part.Cost,
        //        Core = part.Core,
        //        Retail = part.Retail,
        //        TechPayType = part.TechPayType,
        //        TechPayAmount = part.TechPayAmount,
        //        LineCode = part.LineCode,
        //        SubLineCode = part.SubLineCode,
        //        Fractional = part.Fractional,
        //        SkillLevel = part.SkillLevel
        //    };
        //}

        public static InventoryPartToRead Transform(InventoryItemPart part)
        {
            if (part != null)
            {
                return new InventoryPartToRead
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

            return null;
        }

        //public static InventoryPart Transform(InventoryPart part)
        //{
        //    return new()
        //    {
        //        List = part.List,
        //        Cost = part.Cost,
        //        Core = part.Core,
        //        Retail = part.Retail,
        //        TechPayType = part.TechPayType,
        //        TechPayAmount = part.TechPayAmount,
        //        LineCode = part.LineCode,
        //        SubLineCode = part.SubLineCode,
        //        Fractional = part.Fractional,
        //        SkillLevel = part.SkillLevel
        //    };
        //}

        public static void CopyWriteDtoToEntity(InventoryPartToWrite partToUpdate, InventoryItemPart part)
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
    }
}
