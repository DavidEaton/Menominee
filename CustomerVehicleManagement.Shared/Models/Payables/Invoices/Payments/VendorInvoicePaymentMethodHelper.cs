using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
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
                ReconcilingVendor = VendorHelper.ConvertReadToWriteDto(payMethod.ReconcilingVendor)
            };
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToWrite paymentMethod, IEnumerable<string> paymentMethods)
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

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToRead paymentMethod, IEnumerable<string> paymentMethods)
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
    }
}
