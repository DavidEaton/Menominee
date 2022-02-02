using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum VendorInvoiceStatus
    {
        Unknown,
        Open,
        [Display(Name = "Sent")]
        ReturnSent,
        Completed,
        Reconciled
    }
}
