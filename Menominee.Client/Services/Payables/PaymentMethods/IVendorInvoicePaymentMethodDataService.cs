using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Payables.Invoices.Payments;

namespace Menominee.Client.Services.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodDataService
    {
        Task<Result<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>> GetAllAsync();
        Task<Result<VendorInvoicePaymentMethodToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(VendorInvoicePaymentMethodToWrite payMethod);
        Task<Result> UpdateAsync(VendorInvoicePaymentMethodToWrite payMethod);
        Task DeleteAsync(long id);
    }
}
