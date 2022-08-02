using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodRepository
    {
        Task AddPaymentMethodAsync(VendorInvoicePaymentMethod payMethod);
        Task<VendorInvoicePaymentMethod> GetPaymentMethodEntityAsync(long id);
        Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id);
        Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetPaymentMethodListAsync();
        void UpdatePaymentMethod(VendorInvoicePaymentMethod payMethod);
        Task DeletePaymentMethodAsync(long id);
        Task<bool> PaymentMethodExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
