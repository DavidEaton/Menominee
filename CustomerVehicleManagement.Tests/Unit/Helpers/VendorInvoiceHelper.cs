using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Helpers
{
    public static class VendorInvoiceHelper
    {
        public static VendorInvoice CreateVendorInvoice()
        {
            Vendor vendor = CreateVendor();
            VendorInvoiceStatus status = VendorInvoiceStatus.Open;
            VendorInvoiceDocumentType documentType = VendorInvoiceDocumentType.Invoice;
            double Total = 0.0;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            return VendorInvoice.Create(vendor, status, documentType, Total, vendorInvoiceNumbers).Value;
        }

        public static IReadOnlyList<string> CreateVendorInvoiceNumbers(Vendor vendor, List<int> invoiceNumbers)
        {
            return invoiceNumbers.Select(invoiceNumber => $"{vendor.Id}{invoiceNumber}").ToList();
        }

        public static List<string> CreateVendorInvoiceNumbersList(Vendor vendor)
        {
            return new List<string>()
            {
                { $"{vendor.Id}{1}" },
                { $"{vendor.Id}{2}" },
                { $"{vendor.Id}{3}" },
            };
        }
    }
}
