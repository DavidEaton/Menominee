using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class VendorInvoicePayment : Entity
    {
        public long InvoiceId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public double Amount { get; set; }
    }

}
