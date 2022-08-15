using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        Task AddInvoiceAsync(VendorInvoice invoice);
        Task<VendorInvoice> GetInvoiceEntityAsync(long id);
        Task<VendorInvoiceToRead> GetInvoiceAsync(long id);
        Task<Vendor> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync();
        Task DeleteInvoiceAsync(long id);
        Task<bool> InvoiceExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
