using CustomerVehicleManagement.Domain.Entities.Payables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentHelper
    {
        public static IList<VendorInvoicePayment> ConvertWriteDtosToEntities(IReadOnlyList<VendorInvoicePaymentMethodToRead> paymentMethods, IList<VendorInvoicePaymentToWrite> payments)
        {
            return payments?.Select(ConvertWriteDtoToEntity(paymentMethods)).ToList()
                ?? new List<VendorInvoicePayment>();
        }

        public static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> ConvertWriteDtoToEntity(IReadOnlyList<VendorInvoicePaymentMethodToRead> paymentMethods)
        {
            var paymentMethodNames = new List<string>();

            return payment =>
                VendorInvoicePayment.Create(
                    VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(
                        VendorInvoicePaymentMethodHelper.ConvertWriteToReadDto
                        (payment, paymentMethods.First(
                            paymentMethod => paymentMethod.Id == payment.PaymentMethodId)),
                        paymentMethodNames),
                    payment.Amount)
                .Value;
        }

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
                    PaymentMethodId = payment.PaymentMethod.Id,
                    Amount = payment.Amount
                };
        }
    }
}
