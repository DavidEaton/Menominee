﻿using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum WarrantyType
    {
        [Display(Name = "New Warranty")]
        NewWarranty,
        [Display(Name = "Guaranteed Replacement")]
        GuaranteedReplacement,
        [Display(Name = "Defective Replacement")]
        DefectiveReplacement
    }
}
