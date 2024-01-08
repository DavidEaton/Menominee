using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Invoices;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Payables.Invoices
{
    public class VendorInvoiceDataService : DataServiceBase<VendorInvoiceDataService>, IVendorInvoiceDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/vendorinvoices";

        public VendorInvoiceDataService(HttpClient httpClient,
            ILogger<VendorInvoiceDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<IReadOnlyList<VendorInvoiceToReadInList>>> GetAllByParametersAsync(ResourceParameters resourceParameters)
        {
            // TODO: Pass resourceParameters, NOT query string built from resourceParameters
            var parameters = $"VendorId={resourceParameters?.VendorId}&Status={resourceParameters?.Status}";
            var errorMessage = "Failed to get all businesses";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<VendorInvoiceToReadInList>>($"{UriSegment}/listing?{parameters}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<VendorInvoiceToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<VendorInvoiceToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<VendorInvoiceToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get Vendor Invoice with id {id}";
            try
            {
                var result = await httpClient.GetFromJsonAsync<VendorInvoiceToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<VendorInvoiceToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<VendorInvoiceToRead>(errorMessage);
            }
        }

        public async Task<Result<PostResponse>> AddAsync(VendorInvoiceToWrite fromCaller)
        {
            var entityType = "Vendor Invoice";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.InvoiceNumber} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result> UpdateAsync(VendorInvoiceToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                invoice => $"{invoice.ToString}",
                invoice => invoice.Id);
        }
    }
}