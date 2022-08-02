using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Id = payMethod.Id,
                Name = payMethod.Name,
                IsActive = payMethod.IsActive,
                IsOnAccountPaymentType = payMethod.IsOnAccountPaymentType,
                IsReconciledByAnotherVendor = payMethod.IsReconciledByAnotherVendor,
                VendorId = payMethod.VendorId ?? null,
                ReconcilingVendor = VendorHelper.ConvertReadToWriteDto(payMethod.ReconcilingVendor)
            };
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToWrite payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Name = payMethod.Name,
                IsActive = payMethod.IsActive,
                IsOnAccountPaymentType = payMethod.IsOnAccountPaymentType,
                IsReconciledByAnotherVendor = payMethod.IsReconciledByAnotherVendor,
                VendorId = payMethod.VendorId ?? null
            };
        }

        public static VendorInvoicePaymentMethodToRead ConvertEntityToReadDto(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Id = payMethod.Id,
                Name= payMethod.Name,
                IsActive = payMethod.IsActive,
                IsOnAccountPaymentType = payMethod.IsOnAccountPaymentType,
                IsReconciledByAnotherVendor = payMethod.IsReconciledByAnotherVendor,
                VendorId = payMethod.VendorId ?? null,
                ReconcilingVendor = VendorHelper.ConvertEntityToReadDto(payMethod.ReconcilingVendor)
            };
        }

        public static void CopyWriteDtoToEntity(VendorInvoicePaymentMethodToWrite payMethodToUpdate, VendorInvoicePaymentMethod payMethod)
        {
            payMethod.Name = payMethodToUpdate.Name;
            payMethod.IsActive = payMethodToUpdate.IsActive;
            payMethod.IsOnAccountPaymentType = payMethodToUpdate.IsOnAccountPaymentType;
            payMethod.IsReconciledByAnotherVendor = payMethodToUpdate.IsReconciledByAnotherVendor;
            payMethod.VendorId = payMethodToUpdate.VendorId ?? null;
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
    }
}
