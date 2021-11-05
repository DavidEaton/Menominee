using IdentityServer4;
using IdentityServer4.Models;
using Menominee.Idp.Configuration;
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
                    "Shop Role",
                    new List<string>() { "shopRole" }),

                new IdentityResource(
                    "subscriptionLevel",
                    "User's Subsription Level",
                    new List<string>() { "subscriptionLevel" }),

                new IdentityResource(
                    "subscribedProducts",
                    "User's subscribed Products",
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
                new ApiScope(name: "menominee-api", displayName: "Menominee API"),
                new ApiScope(name: "ddc-api", displayName: "Dynamic Database Conection API")
                //new ApiScope(name: "tenantId", displayName: "Id of User's Tenant"),
                //new ApiScope(name: "tenantName", displayName: "Name of User's Tenant"),
                //new ApiScope(name: "roles", displayName: "User's Shop Role(s)"),
                //new ApiScope(name: "subscriptionLevel", displayName: "User's Subsription Level"),
                //new ApiScope(name: "subscribedProducts", displayName: "User's subscribed Products")
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
                        "ddc-api",
                        "tenantId",
                        "tenantName",
                        "role",
                        "shopRole",
                        "subscriptionLevel",
                        "subscribedProducts"
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