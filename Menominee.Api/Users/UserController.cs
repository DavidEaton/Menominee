using Menominee.Api.Common;
using Menominee.Shared;
using Menominee.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Users
{
    [Authorize(Policies.CanManageUsers)]
    public class UserController : BaseControllerApplicationUser<UserController>
    {

        public UserController(
            UserContext userContext,
            IMSGraphUserService graphUserService,
            ILogger<UserController> logger
            ) : base(userContext, graphUserService, logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToRead>>> GetUsers()
        {
            // TODO: filter by tenant
            var results = (await GraphUserService.GetUsers());

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult> Post(RegisterUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await GraphUserService.CreateUser(user);

                if (!result.Successful)
                    return BadRequest(result);

                return Ok(result);
            }
            // TODO: enhance this for development: https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-6.0
            return Problem();
        }
    }
}