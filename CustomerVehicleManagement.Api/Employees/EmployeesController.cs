using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policies.CanManageHumanResources)]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EmployeeReadDto>>> GetUsersAsync()
        {
            var list = new List<EmployeeReadDto>()
            {
                { new EmployeeReadDto() { Id = 1, Name="Jane J. Jones", Gender="Female", Hired=DateTime.Today.AddYears(-2), ShopRole = ShopRole.Admin.ToString() } },
                { new EmployeeReadDto() { Id = 2, Name="Kane K. Kones", Gender="Male", Hired=DateTime.Today.AddYears(-3), ShopRole = ShopRole.Advisor.ToString() } },
                { new EmployeeReadDto() { Id = 3, Name="Lane L. Lones", Gender="Female", Hired=DateTime.Today.AddYears(-1), ShopRole = ShopRole.Employee.ToString() } },
                { new EmployeeReadDto() { Id = 4, Name="Hane H. Hones", Gender="Male", Hired=DateTime.Today.AddYears(-4), ShopRole = ShopRole.Owner.ToString() } },
                { new EmployeeReadDto() { Id = 4, Name="Bane B. Bones", Gender="Male", Hired=DateTime.Today.AddYears(-4), ShopRole = ShopRole.Technician.ToString() } }
            };

            return await Task.FromResult(list);
        }
    }
}
