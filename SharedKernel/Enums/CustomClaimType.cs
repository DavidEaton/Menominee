using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Enums
{
    public enum CustomClaimType
    {
        [Display(Name = "Subscription Level")]
        SubscriptionLevel,
        [Display(Name = "Subscribed Products")]
        SubscribedProducts,
        [Display(Name = "Shop Role")]
        ShopRole
    }
}
