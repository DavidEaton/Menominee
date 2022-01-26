using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.Warranties
{
    public class RepairOrderWarrantyToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public int SequenceNumber { get; set; }
        public double Quantity { get; set; }
        public WarrantyType Type { get; set; }
        public string NewWarranty { get; set; }
        public string OriginalWarranty { get; set; }
        public long OriginalInvoiceId { get; set; }

        public static IReadOnlyList<RepairOrderWarrantyToRead> ConvertToDto(IList<RepairOrderWarranty> warranties)
        {
            return warranties
                .Select(warranty =>
                        ConvertToDto(warranty))
                .ToList();
        }

        private static RepairOrderWarrantyToRead ConvertToDto(RepairOrderWarranty warranty)
        {
            if (warranty != null)
            {
                return new RepairOrderWarrantyToRead()
                {
                    Id = warranty.Id,
                    RepairOrderItemId = warranty.RepairOrderItemId,
                    SequenceNumber = warranty.SequenceNumber,
                    Quantity = warranty.Quantity,
                    Type = warranty.Type,
                    NewWarranty = warranty.NewWarranty,
                    OriginalWarranty = warranty.OriginalWarranty,
                    OriginalInvoiceId = warranty.OriginalInvoiceId
                };
            }

            return null;
        }
    }
}
