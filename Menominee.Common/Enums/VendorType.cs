using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum VendorType
    {
        [Display(Name = "Parts Supplier")]
        PartsSupplier,
        [Display(Name = "Payment Reconcile")]
        PaymentReconcile,
        [Display(Name = "Non-Parts Supplier")]
        NonPartsSupplier
    }
}
