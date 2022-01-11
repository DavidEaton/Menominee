using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Services.Payables.Invoices
{
    public interface IVendorInvoiceDataService
    {
        Task<IReadOnlyList<VendorInvoiceToReadInList>> GetAllInvoices();
        Task<VendorInvoiceToRead> GetInvoice(long id);
        //Task<VendorInvoiceToRead> AddInvoice(VendorInvoiceToAdd invoice);
        Task<VendorInvoiceToRead> AddInvoice(VendorInvoiceToWrite invoice);
        //Task UpdateInvoice(VendorInvoiceToEdit invoice, long id);
        Task UpdateInvoice(VendorInvoiceToWrite invoice, long id);
    }
}
