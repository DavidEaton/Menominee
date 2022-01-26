
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.Taxes
{
    public class RepairOrderTaxToRead
    {
        public long Id { get; set; }
        public long RepairOrderId { get; set; }
        public int SequenceNumber { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }

        public static IReadOnlyList<RepairOrderTaxToRead> ConvertToDto(IList<RepairOrderTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertToDto(tax))
                .ToList();
        }

        private static RepairOrderTaxToRead ConvertToDto(RepairOrderTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderTaxToRead()
                {
                    Id = tax.Id,
                    RepairOrderId = tax.RepairOrderId,
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
