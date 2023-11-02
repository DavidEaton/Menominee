using Menominee.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Taxes
{
    public class ServiceTaxHelper
    {
        public static List<RepairOrderServiceTaxToWrite> ConvertReadToWriteDtos(IList<RepairOrderServiceTaxToRead> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderServiceTaxToWrite()
                {
                    PartTax = new PartTaxToWrite()
                    {
                        Amount = tax.PartTax.Amount,
                        Rate = tax.PartTax.Rate
                    },
                    LaborTax = new LaborTaxToWrite()
                    {
                        Amount = tax.LaborTax.Amount,
                        Rate = tax.LaborTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderServiceTaxToWrite>();
        }

        public static IList<RepairOrderServiceTaxToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderServiceTax> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderServiceTaxToRead()
                {
                    Id = tax.Id,
                    PartTax = new PartTaxToRead()
                    {
                        Amount = tax.PartTax.Amount,
                        Rate = tax.PartTax.Rate
                    },
                    LaborTax = new LaborTaxToRead()
                    {
                        Amount = tax.LaborTax.Amount,
                        Rate = tax.LaborTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderServiceTaxToRead>();
        }

        internal static List<RepairOrderServiceTaxToWrite> ConvertToWriteDtos(IReadOnlyList<RepairOrderServiceTax> taxes)
        {
            return taxes?.Select(
                tax =>
                new RepairOrderServiceTaxToWrite()
                {
                    PartTax = new PartTaxToWrite()
                    {
                        Amount = tax.PartTax.Amount,
                        Rate = tax.PartTax.Rate
                    },
                    LaborTax = new LaborTaxToWrite()
                    {
                        Amount = tax.LaborTax.Amount,
                        Rate = tax.LaborTax.Rate
                    }
                }).ToList()
            ?? new List<RepairOrderServiceTaxToWrite>();
        }

        internal static List<RepairOrderServiceTax> ConvertWriteDtosToEntities(List<RepairOrderServiceTaxToWrite> taxes)
        {
            return taxes?.Select(
                technician =>
                RepairOrderServiceTax.Create(
                    PartTax.Create(technician.PartTax.Rate, technician.PartTax.Amount).Value,
                    LaborTax.Create(technician.LaborTax.Rate, technician.LaborTax.Amount).Value)
                .Value
                ).ToList()
            ?? new List<RepairOrderServiceTax>();
        }
    }
}
