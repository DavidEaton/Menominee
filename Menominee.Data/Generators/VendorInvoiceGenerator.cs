using Menominee.Data.Database;
using Menominee.Data.Results;
using Menominee.Domain.Entities.Payables;
using TestingHelperLibrary.Fakers;

namespace Menominee.Data.Generators
{
    public static class VendorInvoiceGenerator
    {
        public static void GenerateData()
        {
            var vendors = Helper.GetVendors();
            GenerateVendorInvoices(vendors);

            foreach (var vendorInvoice in VendorInvoiceGeneratorResult.VendorInvoices)
                Helper.SaveToDatabase(vendorInvoice);
        }

        private static void GenerateVendorInvoices(IReadOnlyList<Vendor> vendors)
        {
            // TODO: Move to appSetting, make settable at runtime
            var CountOfVendorInvoicesToCreate = 25;
            IReadOnlyList<string> vendorInvoiceNumbers = new List<string>();

            var vendorInvoices = VendorInvoiceFaker.MakeVendorInvoiceFakes(
                CountOfVendorInvoicesToCreate,
                vendors,
                vendorInvoiceNumbers);

            VendorInvoiceGeneratorResult.VendorInvoices = vendorInvoices;
        }
    }
}
