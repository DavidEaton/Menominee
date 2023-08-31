using Menominee.Shared.Models.Employees;

namespace Menominee.Client.Services
{
    public interface IEmployeeDataService
    {
        Task<IReadOnlyList<EmployeeToRead>> GetAllEmployees();
    }
}
