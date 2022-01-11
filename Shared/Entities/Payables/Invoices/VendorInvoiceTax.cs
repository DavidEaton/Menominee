using System.ComponentModel.DataAnnotations.Schema;

namespace MenomineePlayWASM.Shared.Entities.Payables.Invoices
{
    public class VendorInvoiceTax : Entity
    {
        //public long Id { get; set; }
        public long InvoiceId { get; set; }
        public int Order { get; set; }
        public int TaxId { get; set; }
        [NotMapped]
        public string TaxName { get; set; }
        public double Amount { get; set; }
    }
}
