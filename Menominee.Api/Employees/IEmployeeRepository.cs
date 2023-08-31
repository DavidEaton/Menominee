using Menominee.Domain.Entities;
using Menominee.Shared.Models.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Employees
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeeEntities();
        Task<List<EmployeeToRead>> GetEmployees();
    }
}
