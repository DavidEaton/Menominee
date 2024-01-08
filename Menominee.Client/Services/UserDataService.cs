using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Users;
using System.Net.Http.Json;

namespace Menominee.Client.Services
{
    public class UserDataService : DataServiceBase<UserDataService>, IUserDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "api/user";
        private readonly IToastService toastService;

        public UserDataService(HttpClient httpClient,
            ILogger<UserDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync()
        {
            var errorMessage = "Failed to get Users";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<UserResponse>>($"{UriSegment}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<UserResponse>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<UserResponse>>(errorMessage);
            }
        }

        public async Task<Result<UserResponse>> GetAsync(string id)
        {
            var errorMessage = $"Failed to get user with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<UserResponse>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<UserResponse>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<UserResponse>(errorMessage);
            }
        }

        public async Task<Result<PostResponse>> RegisterAsync(RegisterUserRequest fromCaller)
        {
            var entityType = "Manufacturer";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Email} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result> UpdateAsync(RegisterUserRequest fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                user => $"{user.ToString}",
                user => user.Id);
        }
    }
}
