﻿using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.PaymentMethods
{
    public interface IVendorInvoicePaymentMethodRepository
    {
        Task AddPaymentMethodAsync(VendorInvoicePaymentMethod payMethod);
        Task<VendorInvoicePaymentMethod> GetPaymentMethodEntityAsync(long id);
        Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id);
        Task<IReadOnlyList<VendorInvoicePaymentMethodToRead>> GetPaymentMethodsAsync();
        Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetPaymentMethodListAsync();
        void DeletePaymentMethod(VendorInvoicePaymentMethod payMethod);
        Task<bool> PaymentMethodExistsAsync(long id);
        Task SaveChangesAsync();
        Task<IReadOnlyList<string>> GetPaymentMethodNamesAsync();
    }
}