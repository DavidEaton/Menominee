using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Vendors;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Payables.Vendors
{
    public class VendorDataService : DataServiceBase<VendorDataService>, IVendorDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/vendors";

        public VendorDataService(HttpClient httpClient,
            ILogger<VendorDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(VendorToWrite fromCaller)
        {
            var entityType = "Vendor";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Name} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<VendorToRead>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all vendors";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<VendorToRead>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<VendorToRead>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<VendorToRead>>(errorMessage);
            }
        }

        public async Task<Result<VendorToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get vendor with id {id}";
            try
            {
                var result = await httpClient.GetFromJsonAsync<VendorToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<VendorToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<VendorToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(VendorToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                business => $"{business.Name}",
                business => business.Id);
        }
    }
}
