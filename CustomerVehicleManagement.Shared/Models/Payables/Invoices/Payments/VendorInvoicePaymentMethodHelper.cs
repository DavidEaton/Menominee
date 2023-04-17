using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodHelper
    {
        public static VendorInvoicePaymentMethodToWrite ConvertReadToWriteDto(VendorInvoicePaymentMethodToRead payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Name = payMethod.Name,
                IsActive = payMethod.IsActive,
                PaymentType = payMethod.PaymentType,
                ReconcilingVendor = payMethod.ReconcilingVendor
            };
        }

        internal static VendorInvoicePaymentMethodToRead ConvertEntityToWriteDto(VendorInvoicePaymentMethod paymentMethod)
        {
            if (paymentMethod is null)
                return null;

            return new()
            {
                Name = paymentMethod.Name,
                IsActive = paymentMethod.IsActive,
                PaymentType = paymentMethod.PaymentType,
                ReconcilingVendor = VendorHelper.ConvertToReadDto(paymentMethod.ReconcilingVendor)
            };
        }
        public static VendorInvoicePaymentMethodToRead ConvertEntityToReadDto(VendorInvoicePaymentMethod payMethod)
        {
            return payMethod is null
                ? null
                : new()
                {
                    Id = payMethod.Id,
                    Name = payMethod.Name,
                    IsActive = payMethod.IsActive,
                    PaymentType = payMethod.PaymentType,
                    ReconcilingVendor = VendorHelper.ConvertToReadDto(payMethod.ReconcilingVendor)
                };
        }

        public static VendorInvoicePaymentMethodToReadInList ConvertEntityToReadInListDto(VendorInvoicePaymentMethod payMethod)
        {
            return payMethod is null
                ? null
                : new()
                {
                    Id = payMethod.Id,
                    Name = payMethod.Name,
                    IsActive = payMethod.IsActive,
                    // may or may not need these...
                    //PaymentType = payMethod.PaymentType,
                    //ReconcilingVendorName = payMethod.ReconcilingVendor?.Name ?? "N/A",
                    PaymentTypeDescription = PaymentMethodDescription(payMethod)
                };
        }

        public static string PaymentMethodDescription(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod.PaymentType != VendorInvoicePaymentMethodType.ReconciledByOtherVendor)
                return EnumExtensions.GetDisplayName(payMethod.PaymentType);

            return "Reconciled By " + (payMethod.ReconcilingVendor?.Name ?? "N/A");
        }

        internal static VendorInvoicePaymentMethodToRead ConvertWriteToReadDto(VendorInvoicePaymentToWrite payment, VendorInvoicePaymentMethodToRead method)
        {
            if (payment.PaymentMethod.Id == method.Id && payment is not null)
                return
                    new VendorInvoicePaymentMethodToRead()
                    {
                        Id = payment.PaymentMethod.Id,
                        Name = method.Name,
                        IsActive = method.IsActive,
                        PaymentType = method.PaymentType,
                        ReconcilingVendor = method.ReconcilingVendor
                    };

            throw new ArgumentException("Unable to ConvertWriteToReadDto");
        }

        public static VendorInvoicePaymentMethodToRead ConvertReadInListToReadDto(VendorInvoicePaymentMethodToReadInList paymentMethod)
        {
            return
            paymentMethod is null
                ? null
                : new()
                {
                    Id = paymentMethod.Id,
                    Name = paymentMethod.Name,
                    IsActive = paymentMethod.IsActive
                    // TODO:
                    // may or may not need this...
                    //PaymentType = paymentMethod.PaymentType
                    //ReconcilingVendor = paymentMethod.ReconcilingVendorName
                };
        }

        internal static DefaultPaymentMethod ConvertReadDtoToEntity(DefaultPaymentMethodToRead defaultPaymentMethod)
        {
            return DefaultPaymentMethod.Create(
                ConvertReadDtoToEntity(defaultPaymentMethod.PaymentMethod),
                defaultPaymentMethod.AutoCompleteDocuments)
                .Value;
        }

        internal static VendorInvoicePaymentMethod ConvertReadDtoToEntity(VendorInvoicePaymentMethodToRead paymentMethod)
        {
            return VendorInvoicePaymentMethod.Create(
                new List<string>(),
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
