using IdentityServer4;
using IdentityServer4.Models;
using Menominee.Idp.Configuration;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace Menominee.Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                new IdentityResource(
                    "tenantId",
                    "Id of User's Tenant",
                    new List<string>() { "tenantId" }),

                new IdentityResource(
                    "tenantName",
                    "Name of User's Tenant",
                    new List<string>() { "tenantName" }),

                new IdentityResource(
                    "role",
                    "Roles",
                    new List<string>() { "role" }),

                new IdentityResource(
                    "shopRole",
                    CustomClaimType.ShopRole.ToString(),
                    new List<string>() { "shopRole" }),

                new IdentityResource(
                    "subscriptionLevel",
                    CustomClaimType.SubscriptionLevel.ToString(),
                    new List<string>() { "subscriptionLevel" }),

                new IdentityResource(
                    "subscribedProducts",
                    CustomClaimType.SubscribedProducts.ToString(),
                    new List<string>() { "subscribedProducts" })
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource(
                    "menominee-api",
                    "Menominee API",
                    new[] { "role" })
                    {
                        Scopes = {
                            "menominee-api",
                            "tenantId",
                            "tenantName",
                            "role",
                            "shopRole",
                            "subscriptionLevel",
                            "subscribedProducts"
                        },
                        ApiSecrets = { new Secret("apisecret".Sha256())}
                    }
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
                        "menominee-api",
                        "tenantId",
                        "tenantName",
                        "role",
                        "shopRole",
                        "subscriptionLevel",
                        "subscribedProducts"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedCorsOrigins = new List<string>()
                    {
                        "https://localhost:44307"
                    },
                    AccessTokenLifetime = int.Parse(StaticConfigurationHelper.AppSetting("AccessTokenLifetime"))
                }
            };
    }
}