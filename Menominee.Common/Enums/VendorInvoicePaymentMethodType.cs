using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum VendorInvoicePaymentMethodType
    {
        Normal,
        [Display(Name = "Charge / On Account")]
        Charge,
        [Display(Name = "Reconciled By Another Vendor")]
        ReconciledByOtherVendor
    }
}
