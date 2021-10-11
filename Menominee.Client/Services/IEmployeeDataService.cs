using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IEmployeeDataService
    {
        Task<IReadOnlyList<EmployeeToRead>> GetAllEmployees();
    }
}
