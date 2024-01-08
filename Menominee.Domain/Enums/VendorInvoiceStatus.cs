using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
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
