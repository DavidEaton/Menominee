using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderItemTaxToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public int SequenceNumber { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }

        public static IReadOnlyList<RepairOrderItemTaxToRead> ConvertToDto(IList<RepairOrderItemTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertToDto(tax))
                .ToList();
        }

        private static RepairOrderItemTaxToRead ConvertToDto(RepairOrderItemTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderItemTaxToRead()
                {
                    Id = tax.Id,
                    RepairOrderItemId = tax.RepairOrderItemId,
                    SequenceNumber = tax.SequenceNumber,
                    TaxId = tax.TaxId,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate,
                    PartTax = tax.PartTax,
                    LaborTax = tax.LaborTax
                };
            }

            return null;
        }
    }
}
