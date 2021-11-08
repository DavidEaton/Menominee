using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using CustomerVehicleManagement.Shared;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using SharedKernel.Enums;
using CustomerVehicleManagement.Api.Handlers;

namespace CustomerVehicleManagement.UnitTests.AuthorizationPolicyTests
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
                new Claim[] { new Claim("ShopRole", ShopRole.Admin.ToString()) }));

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
