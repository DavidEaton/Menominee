using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Taxes;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public static class RepairOrderItemExtensions
    {
        public static RepairOrderItemPartToWrite ToWriteDto(this RepairOrderItemPart part) =>
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
                TechAmount = part.TechAmount,
                ExciseFees = ExciseFeeHelper.ConvertToWriteDtos(part.ExciseFees)
            };

        public static RepairOrderItemLaborToWrite ToWriteDto(this RepairOrderItemLabor labor) =>
            labor is null ? new RepairOrderItemLaborToWrite() : new RepairOrderItemLaborToWrite
            {
                Id = labor.Id,
                LaborAmount = labor.LaborAmount,
                TechAmount = labor.TechAmount
            };

        public static RepairOrderItemPart ToEntity(this RepairOrderItemPartToWrite itemPart) =>
            itemPart is null
                ? null
                : RepairOrderItemPart.Create(
                    itemPart.List,
                    itemPart.Cost,
                    itemPart.Core,
                    itemPart.Retail,
                    (TechAmount)LaborAmount.Create(itemPart.TechAmount?.Type ?? default, itemPart.TechAmount?.Amount ?? default).Value,
                    itemPart.Fractional,
                    itemPart.LineCode,
                    itemPart.SubLineCode,
                    ExciseFeeHelper.ConvertWriteDtosToEntities(itemPart.ExciseFees)
                ).Value;

        public static RepairOrderItemLabor ToEntity(this RepairOrderItemLaborToWrite labor) =>
            labor is null
                ? null
                : RepairOrderItemLabor.Create(
                    labor.LaborAmount,
                    labor.TechAmount
                ).Value;
    }
}
