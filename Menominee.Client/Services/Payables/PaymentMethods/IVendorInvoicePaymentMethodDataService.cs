using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Invoices.Payments;

namespace Menominee.Client.Services.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodDataService
    {
        Task<Result<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>> GetAllAsync();
        Task<Result<VendorInvoicePaymentMethodToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(VendorInvoicePaymentMethodRequest payMethod);
        Task<Result> UpdateAsync(VendorInvoicePaymentMethodRequest payMethod);
        Task DeleteAsync(long id);
    }
}
