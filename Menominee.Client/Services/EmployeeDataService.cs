using CSharpFunctionalExtensions;
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
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger;
        }

        public async Task<Result<IReadOnlyList<EmployeeToRead>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all employees";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<EmployeeToRead>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<EmployeeToRead>>(errorMessage);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<EmployeeToRead>>(errorMessage);
            }
        }
    }
}
