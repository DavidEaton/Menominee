using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum VendorInvoiceItemType
    {
        Purchase,
        Return,
        [Display(Name = "Core Return")]
        CoreReturn,
        [Display(Name = "Defective Return")]
        Defective,
        [Display(Name = "Warrantied Return")]
        Warranty,
        [Display(Name = "Misc. Debit")]
        MiscDebit,
        [Display(Name = "Misc. Credit")]
        MiscCredit,
        [Display(Name = "Balance Forward")]
        BalanceForward
    }
}
