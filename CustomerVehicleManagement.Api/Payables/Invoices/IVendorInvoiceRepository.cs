using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        void AddInvoice(VendorInvoice entity);
        Task<VendorInvoice> GetInvoiceEntityAsync(long id);
        Task<VendorInvoiceToRead> GetInvoiceAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices(ResourceParameters resourceParameters);
        Task<Vendor> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync(ResourceParameters resourceParameters);
        void DeleteInvoice(VendorInvoice entity);
        Task<bool> InvoiceExistsAsync(long id);
        Task SaveChangesAsync();
        Task<IReadOnlyList<string>> GetVendorInvoiceNumbers(long vendorId);
    }
}