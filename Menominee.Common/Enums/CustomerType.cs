using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum CustomerType
    {
        [Display(Name = "Person")]
        Retail,
        Business,
        Fleet,
        [Display(Name = "Billing Center")]
        BillingCenter,
        [Display(Name = "Billing Center - Prepaid")]
        BillingCenterPrepaid,
        Employee
    }
}