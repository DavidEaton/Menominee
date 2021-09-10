using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.CanManageUsers)]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserListDto>>> GetUsersAsync()
        {
            var list = new List<UserListDto>()
            {
                { new UserListDto() { Id = "1", Name="Jane", Email="j@j.j", Role = Role.Admin.ToString(), Username = "j@j.j" } },
                { new UserListDto() { Id = "2", Name="Kane", Email="k@k.k", Role = Role.Admin.ToString(), Username = "k@k.k" } },
                { new UserListDto() { Id = "3", Name="Lane", Email="l@l.l", Role = Role.Admin.ToString(), Username = "l@l.l" } },
                { new UserListDto() { Id = "4", Name="Hane", Email="h@h.h", Role = Role.Admin.ToString(), Username = "h@h.h" } },
            };

            return list;
        }
    }
}
