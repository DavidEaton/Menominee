using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum ClaimType
    {
        [Display(Name = "Subscription Level")]
        SubscriptionLevel,
        [Display(Name = "Subscribed Products")]
        SubscribedProducts,
        [Display(Name = "Shop Role")]
        ShopRole
    }
}
