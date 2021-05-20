// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
             new TestUser
             {
                 SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                 Username = "Frank",
                 Password = "password",

                 Claims = new List<Claim>
                 {
                     new Claim(JwtClaimTypes.Name, "Frank Underwood"),
                     new Claim(JwtClaimTypes.GivenName, "Frank"),
                     new Claim(JwtClaimTypes.FamilyName, "Underwood"),
                     new Claim(JwtClaimTypes.Email, "frank@underwood.com"),
                     new Claim("country", "BE"),
                     new Claim("TenantId", "714514FC-69C5-4125-A100-FF2DEF725449")
                 }
             },
             new TestUser
             {
                 SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                 Username = "Claire",
                 Password = "password",

                 Claims = new List<Claim>
                 {
                     new Claim(JwtClaimTypes.Name, "Bubba Jones"),
                     new Claim(JwtClaimTypes.GivenName, "Bubba"),
                     new Claim(JwtClaimTypes.FamilyName, "Jones"),
                     new Claim(JwtClaimTypes.Email, "bubba@jones.com"),
                     new Claim("country", "US"),
                     new Claim("TenantId", "8772A385-E57F-4DC0-8AA6-4D0C2F9513C0")
                 }
             }
         };

        //public static List<TestUser> Users
        //{
        //    get
        //    {
        //        var address = new
        //        {
        //            street_address = "One Hacker Way",
        //            locality = "Heidelberg",
        //            postal_code = 69118,
        //            country = "Germany"
        //        };

        //        return new List<TestUser>
        //        {
        //            new TestUser
        //            {
        //                SubjectId = "818727",
        //                Username = "alice",
        //                Password = "alice",
        //                Claims =
        //                {
        //                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //                    new Claim(JwtClaimTypes.GivenName, "Alice"),
        //                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
        //                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
        //                    new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
        //                }
        //            },
        //            new TestUser
        //            {
        //                SubjectId = "88421113",
        //                Username = "bob",
        //                Password = "bob",
        //                Claims =
        //                {
        //                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //                    new Claim(JwtClaimTypes.GivenName, "Bob"),
        //                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
        //                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
        //                    new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
        //                }
        //            }
        //        };
        //    }
        //}
    }
}