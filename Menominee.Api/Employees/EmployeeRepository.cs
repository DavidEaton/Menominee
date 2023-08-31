using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Employees;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Employee>> GetEmployeeEntities()
        {
            return await context.Employees
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<EmployeeToRead>> GetEmployees()
        {
            IReadOnlyList<Employee> employees = await context.Employees
                .AsNoTracking()
                .ToListAsync();

            return employees.
                Select(employee => EmployeeHelper.ConvertToReadDto(employee))
                .ToList();
        }
    }
}
