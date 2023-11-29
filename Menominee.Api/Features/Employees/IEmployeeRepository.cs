using Menominee.Domain.Entities;
using Menominee.Shared.Models.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Employees
{
    public interface IEmployeeRepository
    {
        Task<IReadOnlyList<Employee>> GetEntitiesAsync();
        Task<IReadOnlyList<EmployeeToRead>> GetAllAsync();
    }
}
