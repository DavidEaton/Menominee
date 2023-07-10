using Menominee.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderTaxHelper
    {
        public static List<RepairOrderTaxToWrite> CovertReadToWriteDtos(IList<RepairOrderTaxToRead> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderTaxToWrite()
                {
                    LaborTax = new LaborTaxToWrite()
                    {
                        Amount = tax.LaborTax.Amount,
                        Rate = tax.LaborTax.Rate
                    },

                    PartTax = new PartTaxToWrite()
                    {
                        Rate = tax.PartTax.Rate,
                        Amount = tax.PartTax.Amount
                    },
                }
            ).ToList()
            ?? new List<RepairOrderTaxToWrite>();
        }

        public static List<RepairOrderTaxToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderTax> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderTaxToRead()
                {
                    Id = tax.Id,
                    LaborTax = new LaborTaxToRead()
                    {
                        Amount = tax.LaborTax.Amount,
                        Rate = tax.LaborTax.Rate
                    },
                    PartTax = new PartTaxToRead()
                    {
                        Rate = tax.PartTax.Rate,
                        Amount = tax.PartTax.Amount
                    }
                }).ToList()
            ?? new List<RepairOrderTaxToRead>();
        }

        public static List<RepairOrderTax> ConvertWriteDtosToEntities(IReadOnlyList<RepairOrderTaxToWrite> taxes)
        {
            return taxes?.Select(
            tax =>
            RepairOrderTax.Create(
                PartTax.Create(
                    tax.PartTax.Rate,
                    tax.PartTax.Amount)
                .Value,
                LaborTax.Create(
                    tax.PartTax.Rate,
                    tax.PartTax.Amount)
                .Value)
            .Value)
            .ToList()
            ?? new List<RepairOrderTax>();
        }
    }
}