using Menominee.Api.Handlers;
using Menominee.Common.Enums;
using Menominee.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.AuthorizationPolicyTests
{
    public class CanManageUsersPolicyShould
    {
        [Fact]
        public async Task AllowIfShopRoleAdminIsPresent()
        {
            // Arrange
            var authorizationService = BuildAuthorizationService(services =>
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.CanManageUsers, Policies.CanManageUsersPolicy());
                });
            });
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim(ClaimType.ShopRole.ToString(), ShopRole.Admin.ToString()) }));

            // Act
            var allowed = await authorizationService.AuthorizeAsync(user, "CanManageUsersPolicy");

            // Assert
            Assert.True(allowed.Succeeded);
        }

        [Fact]
        public async Task AllowIfShopRoleOwnerIsPresent()
        {
            // Arrange
            var authorizationService = BuildAuthorizationService(services =>
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.CanManageUsers, Policies.CanManageUsersPolicy());
                });
            });
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim("ShopRole", ShopRole.Owner.ToString()) }));

            // Act
            var allowed = await authorizationService.AuthorizeAsync(user, "CanManageUsersPolicy");

            // Assert
            Assert.True(allowed.Succeeded);
        }

        private IAuthorizationService BuildAuthorizationService(
            Action<IServiceCollection> setupServices = null)
        {
            var services = new ServiceCollection();
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            services.AddLogging();
            services.AddOptions();
            setupServices?.Invoke(services);
            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }
    }
}
