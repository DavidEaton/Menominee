using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ReconcilingVendor = VendorHelper.ConvertReadToWriteDto(payMethod.ReconcilingVendor)
            };
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToWrite payMethod)
        {
            if (payMethod is null)
                return null;

            return VendorInvoicePaymentMethod.Create(
                payMethod.Name,
                payMethod.IsActive,
                payMethod.IsOnAccountPaymentType,
                VendorHelper.ConvertWriteDtoToEntity(payMethod.ReconcilingVendor)
                ).Value;
        }

        public static VendorInvoicePaymentMethodToRead ConvertEntityToReadDto(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
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
            if (payMethod is null)
                return null;

            return new()
            {
                Id = payMethod.Id,
                Name = payMethod.Name,
                IsActive = payMethod.IsActive,
                ReconcilingVendorName = payMethod.ReconcilingVendor?.Name ?? "N/A"
            };
        }

        public static List<VendorInvoicePayment> ConvertPaymentWriteDtosToEntities(IList<VendorInvoicePaymentToWrite> payments)
        {
            return payments?.Select(ConvertPaymentWriteDtoToEntity()).ToList()
                ?? new List<VendorInvoicePayment>();
        }

        public static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> ConvertPaymentWriteDtoToEntity()
        {
            return payment =>
                VendorInvoicePayment.Create(
                VendorInvoicePaymentMethod.Create(
                    payment.PaymentMethod.Name,
                    payment.PaymentMethod.IsActive,
                    payment.PaymentMethod.IsOnAccountPaymentType,
                    VendorHelper.ConvertWriteDtoToEntity(
                        payment.PaymentMethod.ReconcilingVendor)
                    ).Value,
                payment.Amount).Value;
        }
    }
}
