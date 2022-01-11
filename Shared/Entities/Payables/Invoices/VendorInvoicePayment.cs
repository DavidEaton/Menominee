using System.ComponentModel.DataAnnotations.Schema;

namespace MenomineePlayWASM.Shared.Entities.Payables.Invoices
{
    public class VendorInvoicePayment : Entity
    {
        //public long Id { get; set; }
        public long InvoiceId { get; set; }
        public int PaymentMethod { get; set; }
        [NotMapped]
        public string PaymentMethodName { get; set; }
        public double Amount { get; set; }
    }
}
