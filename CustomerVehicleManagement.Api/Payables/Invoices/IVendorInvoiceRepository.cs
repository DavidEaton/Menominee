using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        void AddInvoice(VendorInvoice invoice);
        Task<VendorInvoice> GetInvoiceEntityAsync(long id);
        Task<VendorInvoiceToRead> GetInvoiceAsync(long id);
        Task<Vendor> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync();
        void DeleteInvoice(VendorInvoice entity);
        Task<bool> InvoiceExistsAsync(long id);
        Task SaveChangesAsync();
    }
}
