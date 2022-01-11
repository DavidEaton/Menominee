using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.Settings.Enums
{
    public enum Country
    {
        US,
        [Display(Name = "Canada")]
        CA,
        [Display(Name = "Mexico")]
        MEX
    }
}
