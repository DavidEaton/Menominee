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
    public class AccountController : ControllerBase
    {
        private readonly IdentityUserDbContext Context;
        private readonly UserContext UserContext;
        private readonly UserManager<ApplicationUser> UserManager;
        public AccountController(UserManager<ApplicationUser> userManager,
                                 IdentityUserDbContext context,
                                 UserContext userContext)
        {
            UserManager = userManager;
            Context = context;
            UserContext = userContext;
        }

        [HttpGet("users")]
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
                    ShopRole = u.ShopRole
                };

                users.Add(user);
            }
            return Ok(users);
        }
        public async Task<Guid> GetTenantId()
        {
            var loggedInUser = await UserManager.GetUserAsync(User);
            return loggedInUser.TenantId;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = UserManager.Users.ToList();

            if (users != null)
            {
                var userLookups = new List<UserListDto>();
                foreach (var user in users)
                {
                    var userLookup = new UserListDto
                    {
                        Id = user.Id,
                        Username = user.UserName
                    };
                    userLookups.Add(userLookup);
                }

                return Ok(userLookups);
            }
            else
                return NotFound();
        }
    }
}
