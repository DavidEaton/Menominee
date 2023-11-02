using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.Taxes;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public static class RepairOrderItemExtensions
    {
        public static RepairOrderItemPartToWrite ToRequest(this RepairOrderItemPart part) =>
            part is null ? new RepairOrderItemPartToWrite() : new RepairOrderItemPartToWrite
            {
                Core = part.Core,
                Cost = part.Cost,
                Fractional = part.Fractional,
                Id = part.Id,
                LineCode = part.LineCode,
                List = part.List,
                Retail = part.Retail,
                SubLineCode = part.SubLineCode,
                TechAmount = new TechAmountToRead()
                {
                    Amount = part.TechAmount.Amount,
                    PayType = part.TechAmount.Type,
                    SkillLevel = part.TechAmount.SkillLevel
                },
                ExciseFees = ExciseFeeHelper.ConvertToWriteDtos(part.ExciseFees)
            };

        public static RepairOrderItemLaborToWrite ToRequest(this RepairOrderItemLabor labor)
        {
            return labor is null
                ? new()
                : new()
                {
                    Id = labor.Id,
                    LaborAmount = new LaborAmountToWrite
                    {
                        Amount = labor.LaborAmount.Amount,
                        PayType = labor.LaborAmount.Type
                    },
                    TechAmount = new TechAmountToWrite
                    {
                        Amount = labor.TechAmount.Amount,
                        PayType = labor.TechAmount.Type,
                        SkillLevel = labor.TechAmount.SkillLevel
                    }
                };
        }

        public static RepairOrderItemPart ToEntity(this RepairOrderItemPartToWrite itemPart) =>
            itemPart is null
                ? null
                : RepairOrderItemPart.Create(
                    itemPart.List,
                    itemPart.Cost,
                    itemPart.Core,
                    itemPart.Retail,
                    (TechAmount)LaborAmount.Create(
                        itemPart.TechAmount?.PayType ?? default,
                        itemPart.TechAmount?.Amount ?? default)
                    .Value,
                    itemPart.Fractional,
                    itemPart.LineCode,
                    itemPart.SubLineCode,
                    ExciseFeeHelper.ConvertWriteDtosToEntities(itemPart.ExciseFees)
                ).Value;

        public static RepairOrderItemLabor ToEntity(this RepairOrderItemLaborToWrite labor) =>
            labor is null
                ? null
                : RepairOrderItemLabor.Create(
                    LaborAmount.Create(
                        labor.LaborAmount.PayType,
                        labor.LaborAmount.Amount).Value,
                    TechAmount.Create(
                        labor.TechAmount.PayType,
                        labor.TechAmount.Amount,
                        labor.TechAmount.SkillLevel).Value
                    ).Value;
    }
}
