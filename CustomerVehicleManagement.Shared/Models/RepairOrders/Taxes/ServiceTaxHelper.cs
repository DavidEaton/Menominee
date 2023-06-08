using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class ServiceTaxHelper
    {
        public static List<RepairOrderServiceTaxToWrite> CovertReadToWriteDtos(IList<RepairOrderServiceTaxToRead> taxes)
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
    }
}
