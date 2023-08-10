using System;
using System.ComponentModel.DataAnnotations;


namespace Menominee.Domain.Entities.Settings
{
    public enum SettingName
    {
        [Display(Name = "Decline Parts")]
        DeclineParts,

        [Display(Name = "Months To Retain")]
        MonthsToRetain,

        [Display(Name = "Show After Customer Lookup")]
        ShowAfterCustomerLookup
    }
}
