using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        void Add(VendorInvoice entity);
        void Delete(VendorInvoice entity);
        Task<VendorInvoice> GetEntityAsync(long id);
        Task<VendorInvoiceToRead> GetAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToRead>> GetByParametersAsync(ResourceParameters resourceParameters);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetListByParametersAsync(ResourceParameters resourceParameters);
        Task SaveChangesAsync();
        Task<IReadOnlyList<string>> GetVendorInvoiceNumbersAsync(long vendorId);
    }
}