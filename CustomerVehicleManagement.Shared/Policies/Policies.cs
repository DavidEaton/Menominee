using Microsoft.AspNetCore.Authorization;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared
{
    // removed ".Policies" from namespace to avoid drilling into it twice
    public static class Policies
    {
        // Settle on a naming convention
        public const string CanManageUsers = "CanManageUsers";
        public const string TechniciansUser = "TechniciansUser";
        public const string FreeUser = "FreeUser";
        public const string PaidUser = "PaidUser";
        public const string CanManageHumanResources = "CanManageHumanResources";
        public const string Admin = "Admin";
        public const string Owner = "Owner";

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

        public static AuthorizationPolicy PaidUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
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

        public static AuthorizationPolicy TechniciansUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Technician.ToString(),
                                                  ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }

        public static AuthorizationPolicy OwnerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ShopRole", new[] { ShopRole.Owner.ToString() })
                .Build();
        }
    }
}
