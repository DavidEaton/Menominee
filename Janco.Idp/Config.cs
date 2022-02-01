using IdentityServer4;
using IdentityServer4.Models;
using Menominee.Common.Enums;
using Janco.Idp.Configuration;
using System.Collections.Generic;

namespace Janco.Idp
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
                    ClaimType.ShopRole.ToString(),
                    new List<string>() { "shopRole" }),

                new IdentityResource(
                    "subscriptionLevel",
                    ClaimType.SubscriptionLevel.ToString(),
                    new List<string>() { "subscriptionLevel" }),

                new IdentityResource(
                    "subscribedProducts",
                    ClaimType.SubscribedProducts.ToString(),
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
    }
}