using Menominee.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodToWrite
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //public bool IsOnAccountPaymentType { get; set; }
        public VendorInvoicePaymentMethodType PaymentType { get; set; }
        public VendorToRead ReconcilingVendor { get; set; }
    }
}
