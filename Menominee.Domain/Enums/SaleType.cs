using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum SaleType
    {
        Regular,
        Counter,
        [Display(Name = "Guaranteed Replacement")]
        GuaranteedReplacement,
        [Display(Name = "Defective Replacement")]
        DefectiveReplacement,
        [Display(Name = "Core Return")]
        CoreReturn
    }
}
