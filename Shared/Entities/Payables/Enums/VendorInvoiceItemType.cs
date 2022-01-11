using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Payables.Enums
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
