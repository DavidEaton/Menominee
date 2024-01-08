using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum VendorInvoiceLineItemType
    {
        Purchase,
        Return,
        [Display(Name = "Core Return")]
        CoreReturn,
        [Display(Name = "Defective Return")]
        Defective,
        [Display(Name = "Warranty Return")]
        Warranty,
        [Display(Name = "Misc. Debit")]
        MiscDebit,
        [Display(Name = "Misc. Credit")]
        MiscCredit,
        [Display(Name = "Balance Forward")]
        BalanceForward
    }
}
