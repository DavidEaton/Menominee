using CustomerVehicleManagement.Shared.Models.Payables.Vendors;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ReconcilingVendorName { get; set; }
    }
}
