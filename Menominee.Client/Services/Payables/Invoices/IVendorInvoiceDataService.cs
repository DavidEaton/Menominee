using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Payables.Invoices;

namespace Menominee.Client.Services.Payables.Invoices
{
    public interface IVendorInvoiceDataService
    {
        Task<Result<IReadOnlyList<VendorInvoiceToReadInList>>> GetAllByParametersAsync(ResourceParameters resourceParameters);
        Task<Result<VendorInvoiceToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(VendorInvoiceToWrite invoice);
        Task<Result> UpdateAsync(VendorInvoiceToWrite invoice);
    }
}
