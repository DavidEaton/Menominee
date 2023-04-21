using System.Linq;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;

namespace CustomerVehicleManagement.Tests.Integration
{
    public class VendorTestHelper : TestingHelperLibrary.Payables.VendorTestHelper
    {

        public static VendorInvoicePaymentMethod GetFirstOrDefaultVendorInvoicePaymentMethod()
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                return context.VendorInvoicePaymentMethods.FirstOrDefault();
            }
        }

        public static SalesTax GetFirstOrDefaultSalesTax()
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                return context.SalesTaxes.FirstOrDefault();
            }
        }

        public static Vendor GetFirstOrDefaultVendor()
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                return context.Vendors.FirstOrDefault();
            }
        }
    }
}
