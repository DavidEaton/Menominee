using CustomerVehicleManagement.Shared;
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
                { new UserListDto() { Id = "1", Name="Jane", Email="j@j.j", Role = ShopRole.Admin.ToString(), Username = "j@j.j" } },
                { new UserListDto() { Id = "2", Name="Kane", Email="k@k.k", Role = ShopRole.Advisor.ToString(), Username = "k@k.k" } },
                { new UserListDto() { Id = "3", Name="Lane", Email="l@l.l", Role = ShopRole.Employee.ToString(), Username = "l@l.l" } },
                { new UserListDto() { Id = "4", Name="Hane", Email="h@h.h", Role = ShopRole.Owner.ToString(), Username = "h@h.h" } },
            };

            return await Task.FromResult(list);
        }
    }
}
