using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePaymentMethod : Entity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnAccountPaymentType { get; set; }
        public bool IsReconciledByAnotherVendor { get; set; }
        public long? VendorId { get; set; }
        public Vendor ReconcilingVendor { get; set; }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePaymentMethod() { }

        #endregion
    }
}
