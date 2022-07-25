using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePayment : Entity
    {
        public long VendorInvoiceId { get; set; }
        public virtual VendorInvoicePaymentMethod PaymentMethod { get; set; }
        //public int PaymentMethod { get; set; }
        public double Amount { get; set; }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePayment() { }

        #endregion
    }
}
