using CustomerVehicleManagement.Shared.Models.Payables.Vendors;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnAccountPaymentType { get; set; }
        public bool IsReconciledByAnotherVendor { get; set; }
        public long? VendorId { get; set; }
        public VendorToRead ReconcilingVendor { get; set; }

    }
}
