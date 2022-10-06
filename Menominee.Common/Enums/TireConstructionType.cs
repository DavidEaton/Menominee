using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum TireConstructionType
    {
        [Display(Name = "Radial Construction")]
        R,
        [Display(Name = "Bias (diagonal pile) Construction")]
        D,
        [Display(Name = "Run-flat Construction")]
        F
    }
}
