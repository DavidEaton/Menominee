using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Payables.Invoices.Payments
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

        public static IReadOnlyList<VendorInvoicePayment> ConvertWriteDtosToEntities(IList<VendorInvoicePaymentToWrite> payments)
        {
            return payments.Select(payment =>
                VendorInvoicePayment.Create(
                    ConvertWriteDtoToEntity(payment.PaymentMethod),
                    payment.Amount).Value
            ).ToList();
        }

        // TODO: Add parameter IList<string> paymentMethodNames for validation
        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToRead paymentMethod)
        {
            IList<string> paymentMethodNames = new List<string>();
            return VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames,
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
