using CustomerVehicleManagement.Domain.Entities.Payables;
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
                Name = payMethod.Name
            };
        }

        public static VendorInvoicePaymentMethod ConvertWriteDtoToEntity(VendorInvoicePaymentMethodToWrite payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Name = payMethod.Name
            };
        }

        public static VendorInvoicePaymentMethodToRead ConvertEntityToReadDto(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Id = payMethod.Id,
                Name= payMethod.Name
            };
        }

        public static void CopyWriteDtoToEntity(VendorInvoicePaymentMethodToWrite payMethodToUpdate, VendorInvoicePaymentMethod payMethod)
        {
            payMethod.Name = payMethodToUpdate.Name;
        }

        public static VendorInvoicePaymentMethodToReadInList ConvertEntityToReadInListDto(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is null)
                return null;

            return new()
            {
                Id = payMethod.Id,
                Name = payMethod.Name
            };
        }
    }
}
