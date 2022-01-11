using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public interface IVendorInvoiceRepository
    {
        Task AddInvoiceAsync(VendorInvoice invoice);
        Task<VendorInvoice> GetInvoiceEntityAsync(long id);
        Task<VendorInvoiceToRead> GetInvoiceAsync(long id);
        //Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoicesAsync();
        Task<Vendor> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync();
        void UpdateInvoiceAsync(VendorInvoice invoice);
        Task DeleteInvoiceAsync(long id);
        Task<bool> InvoiceExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();

        ////Task AddInvoiceAsync(VendorInvoice entity);
        ////Task<int> CreateInvoiceAsync(VendorInvoiceCreateDto vendorInvoice);
        ////void DeleteInvoice(VendorInvoice invoice);
        //void FixTrackingState();
        //Task<bool> InvoiceExistsAsync(int id);
        ////Task UpdateInvoiceAsync(VendorInvoiceUpdateDto invoice);
        //Task UpdateInvoiceAsync(VendorInvoiceToEdit invoice);
        //Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoicesAsync();
        //Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync();
        //Task<VendorInvoiceToRead> GetInvoiceAsync(int id);
        ////Task<VendorInvoice> GetVendorInvoiceEntityAsync(int id);
        //Task<bool> SaveChangesAsync();
    }
}
