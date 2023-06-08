using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public interface IVendorInvoiceRepository
    {
        Task Add(VendorInvoice entity);
        Task<VendorInvoice> GetEntity(long id);
        Task<VendorInvoiceToRead> Get(long id);
        Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices(ResourceParameters resourceParameters);
        Task<Vendor> GetVendor(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetList(ResourceParameters resourceParameters);
        void Delete(VendorInvoice entity);
        Task<bool> Exists(long id);
        Task SaveChanges();
        Task<IReadOnlyList<string>> GetVendorInvoiceNumbers(long vendorId);
    }
}