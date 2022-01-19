using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Data;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Janco.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Users
{
    [Authorize(Policies.CanManageUsers)]
    public class UsersController : ApplicationController
    {
        private readonly IdentityUserDbContext dbContext;
        private readonly UserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IdentityUserDbContext dbContext, UserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<UserToRead>> GetUsers()
        {
            var tenantId = GetTenantId();

            var users = dbContext.ApplicationUsers.Where(appUser => appUser.TenantId == tenantId).Select(user => new UserToRead()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                Username = user.UserName,
                ShopRole = user.ShopRole

            }).ToList();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegisterUser user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                TenantId = GetTenantId(),
                TenantName = GetTenantName(),
                ShopRole = user.ShopRole,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
                return BadRequest(new RegisterUserResult
                {
                    Successful = false,
                    Errors = result.Errors.Select(x => x.Description)
                });

            return Ok(new RegisterUserResult { Successful = true });
        }

        private Guid GetTenantId()
        {
            if (userContext == null)
                return new Guid();

            var claims = userContext.Claims;

            try
            {
                return Guid.Parse(claims.First(claim => claim.Type == "tenantId").Value);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from GetTenantId(): {ex.Message}");
                return new Guid();
            }
        }

        private string GetTenantName()
        {
            if (userContext == null)
                return string.Empty;

            var claims = userContext.Claims;

            try
            {
                return claims.First(claim => claim.Type == "tenantName").Value;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception message from GetTenantId(): {ex.Message}");
                return string.Empty;
            }
        }

    }
}

