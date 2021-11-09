using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Users
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.CanManageUsers)]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserContext UserContext;
        private readonly UserManager<ApplicationUser> UserManager;
        public AccountsController(UserManager<ApplicationUser> userManager,
                                  UserContext userContext)
        {
            UserManager = userManager;
            UserContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetIdentityUsersAsync()
        {
            var tenantId = GetTenantId();

            var users = new List<UserListDto>();
            var identityUsers = UserManager.Users.Where(u => u.TenantId == tenantId).ToList();

            foreach (var u in identityUsers)
            {
                var user = new UserListDto
                {
                    Email = u.Email,
                    //Name = u.Name,
                    Username = u.UserName,
                    Id = u.Id,
                    ShopRole = u.ShopRole.ToString()
                };

                users.Add(user);
            }
            return Ok(users);
        }
        private Guid GetTenantId()
        {
            if (UserContext == null)
                return new Guid();

            var claims = UserContext.Claims;
            Guid tenantId;

            try
            {
                tenantId = Guid.Parse(claims.First(claim => claim.Type == "tenantId").Value);
            }
            catch (Exception ex)
            {
                // TODO: Logger.LogError($"Exception message from GetTenantId(): {ex.Message}");
                return new Guid();
            }

            return tenantId;
        }

        private string GetTenantName()
        {
            if (UserContext == null)
                return string.Empty;

            var claims = UserContext.Claims;

            try
            {
                return (claims.First(claim => claim.Type == "tenantName").Value);
            }
            catch (Exception ex)
            {
                // TODO: Logger.LogError($"Exception message from GetTenantId(): {ex.Message}");
                return string.Empty;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(RegisterUser model)
        {
            var newUser = new ApplicationUser {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                TenantId = GetTenantId(),
                TenantName = GetTenantName(),
                ShopRole = ShopRole.Technician
            };

            var result = await UserManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterUserResult { Successful = false, Errors = errors });

            }

            return Ok(new RegisterUserResult { Successful = true });
        }

    }
}
