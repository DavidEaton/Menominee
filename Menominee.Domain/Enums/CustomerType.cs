using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum CustomerType
    {
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