using System;

namespace MenomineePlayWASM.Shared.Entities.Payables.CreditReturns
{
    public class CreditReturnItem
    {
        public long Id { get; set; }
        public string InvoiceId { get; set; }
        public int Type { get; set; }
        public string PartNumber { get; set; }
        public string MfrId { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public string PONumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
