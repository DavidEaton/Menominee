using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Vendors;

namespace Menominee.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        //public bool IsOnAccountPaymentType { get; set; }
        public VendorInvoicePaymentMethodType PaymentType { get; set; } = VendorInvoicePaymentMethodType.Normal;
        public VendorToRead ReconcilingVendor { get; set; }
    }
}
