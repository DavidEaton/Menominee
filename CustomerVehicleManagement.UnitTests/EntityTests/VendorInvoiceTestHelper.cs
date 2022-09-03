using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public static class VendorInvoiceTestHelper
    {
        public static Vendor CreateVendor()
        {
            return Vendor.Create(
                Utilities.RandomCharacters(Vendor.MinimumLength),
                Utilities.RandomCharacters(Vendor.MinimumLength)).Value;
        }

        public static IList<string> CreatePaymentMethodNames()
        {
            IList<string> paymentMethodNames = new List<string>();
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength));
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 10));
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 20));
            return paymentMethodNames;
        }

        public static VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor).Value;
        }

        public static VendorInvoicePayment CreateVendorInvoicePayment()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            double amount = VendorInvoicePayment.InvalidValue + 1;
            return VendorInvoicePayment.Create(paymentMethod, amount).Value;
        }
    }
}