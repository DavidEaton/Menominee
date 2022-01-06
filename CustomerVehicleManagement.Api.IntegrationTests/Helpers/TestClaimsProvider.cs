using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin user"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return provider;
        }

        public static TestClaimsProvider WithShopAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Shop Admin user"));
            provider.Claims.Add(new Claim(ClaimType.ShopRole.ToString(), ShopRole.Admin.ToString()));

            return provider;
        }

        public static TestClaimsProvider WithHumanResourcesClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Shop HumanResources user"));
            provider.Claims.Add(new Claim(ClaimType.ShopRole.ToString(), ShopRole.HumanResources.ToString()));

            return provider;
        }

        public static TestClaimsProvider WithShopOwnerClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Shop Owner user"));
            provider.Claims.Add(new Claim(ClaimType.ShopRole.ToString(), ShopRole.Owner.ToString()));

            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));

            return provider;
        }
    }
}