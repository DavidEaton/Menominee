using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenomineePlayWASM.Shared.Entities.Payables.CreditReturns
{
    public class CreditReturn
    {
        public long Id { get; set; }
        public long VendorId { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public string DateAsString { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }

        public List<CreditReturnItem> LineItems { get; set; } = new List<CreditReturnItem>();
    }
}
