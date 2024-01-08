using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum SubscriptionLevel
    {
        //These are tenant-specific (not user)
        Free,
        [Display(Name = "Paid User")]
        PaidUser
    }
}
