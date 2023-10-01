using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Employees;

namespace Menominee.Client.Services
{
    public interface IEmployeeDataService
    {
        Task<Result<IReadOnlyList<EmployeeToRead>>> GetAllAsync();
    }
}
