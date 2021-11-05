using Microsoft.AspNetCore.Authorization;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared
{
    // removed ".Policies" from namespace to avoid drilling into it twice
    public static class Policies
    {
        // TODO: Settle on a naming convention
        public const string AdminOnly = "AdminPolicy";
        public const string CanManageHumanResources = "CanManageHumanResourcesPolicy";
        public const string CanManageUsers = "CanManageUsersPolicy";
        public const string FreeUser = "FreeUserPolicy";
        public const string OwnerOnly = "OwnerPolicy";
        public const string PaidUser = "PaidUserPolicy";
        public const string TechniciansUser = "TechniciansUserPolicy";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy CanManageHumanResourcesPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.HumanResources.ToString(),
                                                  ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy CanManageUsersPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy FreeUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser() // Free users have extremely limited features, but still need an account
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.Free.ToString() })
                .Build();
        }

        public static AuthorizationPolicy OwnerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy PaidUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .Build();
        }

        public static AuthorizationPolicy TechnicianUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Technician.ToString(),
                                                  ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }
    }
}
