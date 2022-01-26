using MenomineePlayWASM.Shared.Dtos.RepairOrders.Payments;
using MenomineePlayWASM.Shared.Dtos.RepairOrders.Services;
using MenomineePlayWASM.Shared.Dtos.RepairOrders.Taxes;
using System;
using System.Collections.Generic;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders
{
    public class RepairOrderToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderNumber { get; set; } = 0;
        public long InvoiceNumber { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public double Total { get; set; } = 0.0;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateInvoiced { get; set; }

        public IList<RepairOrderServiceToWrite> Services { get; set; } = new List<RepairOrderServiceToWrite>();
        public IList<RepairOrderTaxToWrite> Taxes { get; set; } = new List<RepairOrderTaxToWrite>();
        public IList<RepairOrderPaymentToWrite> Payments { get; set; } = new List<RepairOrderPaymentToWrite>();

    }
}
