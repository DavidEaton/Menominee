using Menominee.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVehicleManagement.Shared
{
    public static class Policies
    {
        // TODO: Settle on a naming convention
        public const string IsAdmin = "IsAdmin";
        public const string CanManageHumanResources = "CanManageHumanResourcesPolicy";
        public const string CanManageUsers = "CanManageUsersPolicy";
        public const string IsFree = "IsFree";
        public const string IsOwner = "IsOwner";
        public const string IsPaid = "IsPaid";
        public const string IsTechnician = "IsTechnician";
        public const string IsAuthenticated = "IsAuthenticated";

        public const string RoleClaimKey = "extension_shopRole";
        
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(RoleClaimKey, new[] { ShopRole.Admin.ToString(), ShopRole.Owner.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy RequireAuthenticatedUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }
        
        public static AuthorizationPolicy CanManageHumanResourcesPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(RoleClaimKey, new[] { ShopRole.HumanResources.ToString(),
                                                  ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy CanManageUsersPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(RoleClaimKey, new[] { ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy FreeUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser() // Free users have extremely limited features, but still need an account
                                            //.RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.Free.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy OwnerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(RoleClaimKey, new[] { ShopRole.Owner.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy PaidUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireClaim("subscriptionLevel", new[] { SubscriptionLevel.PaidUser.ToString() })
                .Build();
        }
        
        public static AuthorizationPolicy TechnicianUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(RoleClaimKey, new[] { ShopRole.Technician.ToString(),
                                                  ShopRole.Admin.ToString(),
                                                  ShopRole.Owner.ToString() })
                .Build();
        }
    }
}
