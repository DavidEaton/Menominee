using Menominee.Shared.Models;
using Menominee.Shared.Models.Employees;
using System.Net.Http.Json;

namespace Menominee.Client.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<EmployeeDataService> logger;
        private const string UriSegment = "api/employees";

        public EmployeeDataService(HttpClient httpClient, ILogger<EmployeeDataService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }
        public async Task<IReadOnlyList<EmployeeToRead>> GetAllEmployees()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<EmployeeToRead>>($"{UriSegment}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all employees");
            }

            return null;
        }
    }
}
