using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderPurchase : Entity
    {
        public long RepairOrderItemId { get; set; }
        public long VendorId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PONumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string VendorPartNumber { get; set; }
    }
}
