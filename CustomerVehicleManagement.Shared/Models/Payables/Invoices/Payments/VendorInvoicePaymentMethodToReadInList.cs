using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        // may or may not need these...
        //public VendorInvoicePaymentMethodType PaymentType { get; set; }
        //public string ReconcilingVendorName { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
}
