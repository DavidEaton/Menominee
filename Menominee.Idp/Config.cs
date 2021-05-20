using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Menominee.Idp
{
    public static class Config
    {
        // Access Tokens issues while running in Development are not refreshed like Production
        // tokens so they need a longer lifetime than only five minutes.
        // Six months seems reasonable.
        private const int AccessTokenLifetime = 15700000;
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("country", new [] { "country"}),
                new IdentityResource("tenantId", new [] { "tenantId"})
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("menominee-api", "Menominee API", new [] { "country", "tenantId" }),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(name: "menominee-api", displayName: "Menominee API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "menominee-client",
                    ClientName = "Menominee",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44307/authentication/login-callback"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44307/authentication/logout-callback"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "menominee-api"
                    },
                    AllowedCorsOrigins = new List<string>()
                    {
                        "https://localhost:44307"
                    },
                    AccessTokenLifetime = AccessTokenLifetime
                }
            };
    }
}