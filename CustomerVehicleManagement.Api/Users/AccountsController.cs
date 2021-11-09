using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Users
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.CanManageUsers)]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetIdentityUsersAsync()
        {
            var tenantId = await GetTenantId();

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
        public async Task<Guid> GetTenantId()
        {
            //var loggedInUser = await UserManager.GetUserAsync(User);
            //return loggedInUser.TenantId;
            Guid tenantId = new("8451406b-8cca-4e2b-ad2c-096a563bc7bc");
            return tenantId;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterUser model)
        {
            var newUser = new ApplicationUser { UserName = model.Email,
                                                Email = model.Email,
                                                EmailConfirmed = true};

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
