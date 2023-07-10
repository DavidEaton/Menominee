using Menominee.Shared.Models.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Invoices
{
    public interface IVendorInvoiceDataService
    {
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoices(ResourceParameters resourceParameters);
        Task<VendorInvoiceToRead> GetInvoice(long id);
        //Task<VendorInvoiceToRead> AddInvoice(VendorInvoiceToAdd invoice);
        Task<VendorInvoiceToRead> AddInvoice(VendorInvoiceToWrite invoice);
        //Task UpdateInvoice(VendorInvoiceToEdit invoice, long id);
        Task UpdateInvoice(VendorInvoiceToWrite invoice, long id);
    }
}
