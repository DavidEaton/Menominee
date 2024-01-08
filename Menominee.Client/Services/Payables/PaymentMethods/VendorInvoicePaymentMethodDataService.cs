using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodDataService : DataServiceBase<VendorInvoicePaymentMethodDataService>, IVendorInvoicePaymentMethodDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/vendorinvoicepaymentmethods";

        public VendorInvoicePaymentMethodDataService(HttpClient httpClient,
            ILogger<VendorInvoicePaymentMethodDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all payment methods";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<VendorInvoicePaymentMethodToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get payment method with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<VendorInvoicePaymentMethodToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<VendorInvoicePaymentMethodToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<VendorInvoicePaymentMethodToRead>(errorMessage);
            }
        }

        public async Task<Result<PostResponse>> AddAsync(VendorInvoicePaymentMethodRequest fromCaller)
        {
            var entityType = "Business";
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

        public async Task<Result> UpdateAsync(VendorInvoicePaymentMethodRequest fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                productCode => $"{productCode.ToString}",
                productCode => productCode.Id);
        }

        public async Task DeleteAsync(long id)
        {
            var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Payment method deleted successfully", "Deleted");
                return;
            }

            toastService.ShowError($"Failed to delete payment method:  Id = {id}", "Delete Failed");
        }
    }
}
