using CustomerVehicleManagement.Data.Database;
using CustomerVehicleManagement.Data.Fakers;
using CustomerVehicleManagement.Data.Results;
using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Data.Generators
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
