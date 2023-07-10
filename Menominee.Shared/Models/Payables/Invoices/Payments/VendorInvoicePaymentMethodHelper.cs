using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentMethodHelper
    {
        public static VendorInvoicePaymentMethodToWrite ConvertReadToWriteDto(VendorInvoicePaymentMethodToRead payMethod)
        {
            return payMethod is null
                ? null
                : new()
                {
                    Name = payMethod.Name,
                    IsActive = payMethod.IsActive,
                    PaymentType = payMethod.PaymentType,
                    ReconcilingVendor = payMethod.ReconcilingVendor
                };
        }

        internal static VendorInvoicePaymentMethodToRead ConvertToWriteDto(VendorInvoicePaymentMethod paymentMethod)
        {
            return paymentMethod is null
                ? null
                : new VendorInvoicePaymentMethodToRead()
                {
                    Id = paymentMethod.Id,
                    Name = paymentMethod.Name,
                    IsActive = paymentMethod.IsActive,
                    PaymentType = paymentMethod.PaymentType,
                    ReconcilingVendor = VendorHelper.ConvertToReadDto(paymentMethod.ReconcilingVendor)
                };
        }
        public static VendorInvoicePaymentMethodToRead ConvertToReadDto(VendorInvoicePaymentMethod payMethod)
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

        public static VendorInvoicePaymentMethodToReadInList ConvertToReadInListDto(VendorInvoicePaymentMethod payMethod)
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
            return payMethod.PaymentType is not VendorInvoicePaymentMethodType.ReconciledByOtherVendor
                ? EnumExtensions.GetDisplayName(payMethod.PaymentType)
                : "Reconciled By " + (payMethod.ReconcilingVendor?.Name ?? "N/A");
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
