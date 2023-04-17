using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentHelper
    {
        public static IList<VendorInvoicePaymentToWrite> ConvertReadDtosToWriteDtos(IReadOnlyList<VendorInvoicePaymentToRead> payments)
        {
            return payments?.Select(ConvertReadDtoToWriteDto()).ToList()
                ?? new List<VendorInvoicePaymentToWrite>();
        }

        public static Func<VendorInvoicePaymentToRead, VendorInvoicePaymentToWrite> ConvertReadDtoToWriteDto()
        {
            return payment =>
                new VendorInvoicePaymentToWrite()
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount
                };
        }

        // TODO: Add parameter IList<string> paymentMethodNames for validation
        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToRead paymentMethod)
        {
            IList<string> paymentMethodNames = new List<string>();
            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                paymentMethod.Name,
                paymentMethod.IsActive,
                paymentMethod.PaymentType,
                paymentMethod.ReconcilingVendor is null
                    ? null
                    : VendorHelper.ConvertReadToEntity(paymentMethod.ReconcilingVendor)
                ).Value;
        }
    }
}
