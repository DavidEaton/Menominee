using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderWarranty : Entity
    {
        public long RepairOrderItemId { get; set; }
        public int SequenceNumber { get; set; }
        public double Quantity { get; set; }
        public WarrantyType Type { get; set; }
        public string NewWarranty { get; set; }
        public string OriginalWarranty { get; set; }
        public long OriginalInvoiceId { get; set; }
    }
}
