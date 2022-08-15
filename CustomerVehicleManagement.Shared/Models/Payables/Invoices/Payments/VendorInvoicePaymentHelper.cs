using CustomerVehicleManagement.Domain.Entities.Payables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentHelper
    {
        public static List<VendorInvoicePayment> ConvertWriteDtosToEntities(IList<VendorInvoicePaymentToWrite> payments)
        {
            return payments?.Select(ConvertWriteDtoToEntity()).ToList()
                ?? new List<VendorInvoicePayment>();
        }

        public static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> ConvertWriteDtoToEntity()
        {
            return payment =>
                VendorInvoicePayment.Create(
                    VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(payment.PaymentMethod),
                    payment.Amount)
                .Value;
        }

        public static VendorInvoicePayment ConvertWriteDtoToEntity(VendorInvoicePaymentToWrite payment)
        {
            if (payment is null)
                return null;

            return VendorInvoicePayment.Create(
                VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(payment.PaymentMethod),
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
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadToWriteDto(payment.PaymentMethod),
                    Amount = payment.Amount
                };
        }
    }
}
