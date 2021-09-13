using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Enums
{
    public enum SubscriptionLevel
    {
        //These are tenant-specific (not user)
        Free,
        [Display(Name = "Paid User")]
        PaidUser
    }
}
