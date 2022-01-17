using CustomerVehicleManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "api/employees";

        public EmployeeDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IReadOnlyList<EmployeeToRead>> GetAllEmployees()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<EmployeeToRead>>($"{UriSegment}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }
    }
}
