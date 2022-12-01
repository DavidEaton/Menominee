using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
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
                IsOnAccountPaymentType = payMethod.IsOnAccountPaymentType,
                ReconcilingVendor = payMethod.ReconcilingVendor
            };
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToWrite paymentMethod, IList<string> paymentMethods)
        {
            return paymentMethod is null
                ? null
                : VendorInvoicePaymentMethod.Create(
                    paymentMethods,
                    paymentMethod.Name,
                    paymentMethod.IsActive,
                    paymentMethod.IsOnAccountPaymentType,
                    VendorHelper.ConvertWriteDtoToEntity(paymentMethod.ReconcilingVendor)
                ).Value;
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToRead paymentMethod, IList<string> paymentMethodNames)
        {
            return paymentMethod is null
                ? null
                : VendorInvoicePaymentMethod.Create(
                    paymentMethodNames,
                    paymentMethod.Name,
                    paymentMethod.IsActive,
                    paymentMethod.IsOnAccountPaymentType,
                    VendorHelper.ConvertWriteDtoToEntity(paymentMethod.ReconcilingVendor)
                ).Value;
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
                    IsOnAccountPaymentType = payMethod.IsOnAccountPaymentType,
                    ReconcilingVendor = VendorHelper.ConvertEntityToReadDto(payMethod.ReconcilingVendor)
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
                    ReconcilingVendorName = payMethod.ReconcilingVendor?.Name ?? "N/A"
                };
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
                        IsOnAccountPaymentType = method.IsOnAccountPaymentType,
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
                    IsActive = paymentMethod.IsActive,
                    //ReconcilingVendor = paymentMethod.ReconcilingVendorName
                };
        }
    }
}
