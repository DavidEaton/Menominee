using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        void AddInvoice(VendorInvoice entity);
        Task<VendorInvoice> GetInvoiceEntityAsync(long id);
        Task<VendorInvoiceToRead> GetInvoiceAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices(long? vendorId, VendorInvoiceStatus? status);
        Task<Vendor> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync();
        void DeleteInvoice(VendorInvoice entity);
        Task<bool> InvoiceExistsAsync(long id);
        Task SaveChangesAsync();
        Task<IReadOnlyList<string>> GetVendorInvoiceNumbers(long vendorId);
    }
}