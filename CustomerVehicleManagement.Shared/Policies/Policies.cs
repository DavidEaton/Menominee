using Microsoft.AspNetCore.Authorization;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared
{
    // removed ".Policies" from namespace to avoid drilling into it twice
    public static class Policies
    {
        public const string CanManageUsers = "CanManageUsers";

        public static AuthorizationPolicy FreeUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser() // Free users have extremely limited features, but still need an account
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.Free.ToString() })
                .Build();
        }

        public static AuthorizationPolicy PaidUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .Build();
        }

        public static AuthorizationPolicy HumanResourcesPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .RequireClaim("role", new[] { ShopRole.HumanResources.ToString(), ShopRole.Admin.ToString(), ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy CanManageUsersPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ShopRole.Admin.ToString())
                .Build();
        }

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .RequireClaim("role", new[] { ShopRole.Admin.ToString(), ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy OwnerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .RequireClaim("role", new[] { ShopRole.Owner.ToString() })
                .Build();
        }

    }
}
