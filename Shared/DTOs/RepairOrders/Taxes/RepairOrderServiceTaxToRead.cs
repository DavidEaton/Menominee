
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.Taxes
{
    public class RepairOrderServiceTaxToRead
    {
        public long Id { get; set; }
        public long RepairOrderServiceId { get; set; }
        public int SequenceNumber { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }

        public static IReadOnlyList<RepairOrderServiceTaxToRead> ConvertToDto(IList<RepairOrderServiceTax> taxes)
        {
            return taxes
                .Select(tax =>
                        ConvertToDto(tax))
                .ToList();
        }

        private static RepairOrderServiceTaxToRead ConvertToDto(RepairOrderServiceTax tax)
        {
            if (tax != null)
            {
                return new RepairOrderServiceTaxToRead()
                {
                    Id = tax.Id,
                    RepairOrderServiceId = tax.RepairOrderServiceId,
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
