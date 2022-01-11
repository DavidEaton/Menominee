using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Payables.Enums
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
