using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePaymentMethod : Entity
    {
        public string Name { get; set; }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePaymentMethod() { }

        #endregion
    }
}
