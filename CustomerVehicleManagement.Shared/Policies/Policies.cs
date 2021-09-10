using Microsoft.AspNetCore.Authorization;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared
{
    // removed ".Policies" from namespace to avoid drilling into it twice
    public static class Policies
    {
        public const string CanManageUsers = "CanManageUsers";
        public static AuthorizationPolicy CanManageUsersPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimType.role.ToString(), new[] { Role.Admin.ToString(), Role.Owner.ToString() })
                .Build();
        }
    }
}
