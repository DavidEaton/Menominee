using Menominee.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Taxes
{
    public class ItemTaxHelper
    {
        public static List<RepairOrderItemTaxToWrite> ConvertReadToWriteDtos(IReadOnlyList<RepairOrderItemTaxToRead> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderItemTaxToWrite()
                {
                    Id = tax.Id,
                    LaborTax = new()
                    {
                        Rate = tax.LaborTax.Rate,
                        Amount = tax.LaborTax.Amount
                    },
                    PartTax = new()
                    {
                        Amount = tax.PartTax.Amount,
                        Rate = tax.PartTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderItemTaxToWrite>();
        }

        public static List<RepairOrderItemTaxToWrite> ConvertWriteToReadDtos(IReadOnlyList<RepairOrderItemTaxToRead> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderItemTaxToWrite()
                {
                    Id = tax.Id,
                    LaborTax = new()
                    {
                        Rate = tax.LaborTax.Rate,
                        Amount = tax.LaborTax.Amount
                    },
                    PartTax = new()
                    {
                        Amount = tax.PartTax.Amount,
                        Rate = tax.PartTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderItemTaxToWrite>();
        }

        public static IReadOnlyList<RepairOrderItemTaxToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderItemTax> taxes)
        {
            return taxes?.Select(
                itemTax =>
                new RepairOrderItemTaxToRead()
                {
                    Id = itemTax.Id,
                    LaborTax = new LaborTaxToRead()
                    {
                        Amount = itemTax.LaborTax.Amount,
                        Rate = itemTax.LaborTax.Rate
                    },
                    PartTax = new PartTaxToRead()
                    {
                        Amount = itemTax.PartTax.Amount,
                        Rate = itemTax.PartTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderItemTaxToRead>();
        }

    }
}
