using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodRepository
    {
        void Add(VendorInvoicePaymentMethod payMethod);
        void Delete(VendorInvoicePaymentMethod entity);
        Task<VendorInvoicePaymentMethod> GetEntityAsync(long id);
        Task<VendorInvoicePaymentMethodToRead> GetAsync(long id);
        Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetListAsync();
        Task SaveChangesAsync();
        Task<IReadOnlyList<string>> GetPaymentMethodNamesAsync();
    }
}
