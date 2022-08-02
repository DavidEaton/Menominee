using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodDataService
    {
        Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetAllPaymentMethodsAsync();
        Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id);
        Task<VendorInvoicePaymentMethodToRead> AddPaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethod);
        Task UpdatePaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethod, long id);
    }
}
