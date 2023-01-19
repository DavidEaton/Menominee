using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerVehicleManagement.Api.Data;

namespace CustomerVehicleManagement.Api.Employees
{
    [Authorize(Policies.CanManageHumanResources)]
    public class EmployeesController : ApplicationController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EmployeeToRead>>> GetUsersAsync()
        {
            var list = new List<EmployeeToRead>()
            {
                { new EmployeeToRead() { Id = 1, Name="Hard-Coded list...", Gender="Female", Hired=DateTime.Today.AddYears(-2), ShopRole = ShopRole.Admin.ToString() } },
                { new EmployeeToRead() { Id = 2, Name="...from EmployeesController", Gender="Male", Hired=DateTime.Today.AddYears(-3), ShopRole = ShopRole.Advisor.ToString() } },
                { new EmployeeToRead() { Id = 3, Name="Lane L. Lones", Gender="Female", Hired=DateTime.Today.AddYears(-1), ShopRole = ShopRole.Employee.ToString() } },
                { new EmployeeToRead() { Id = 4, Name="Hane H. Hones", Gender="Male", Hired=DateTime.Today.AddYears(-4), ShopRole = ShopRole.Owner.ToString() } },
                { new EmployeeToRead() { Id = 4, Name="Bane B. Bones", Gender="Male", Hired=DateTime.Today.AddYears(-4), ShopRole = ShopRole.Technician.ToString() } }
            };

            return await Task.FromResult(list);
        }
    }
}
