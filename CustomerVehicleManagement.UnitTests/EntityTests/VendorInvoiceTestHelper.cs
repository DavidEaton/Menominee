using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.TestUtilities;
using Menominee.Common.Enums;
using System;
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

        internal static List<ExciseFee> CreateExciseFees()
        {
            // TODO: This test method creates and returns an entity list
            // with all Id == 0. That breaks identity comaprisons like
            // if (!ExciseFees.Any(x => x.Id == fee.Id))... inside our
            // domain class SalesTax.SetExciseFees creation/validation
            // MUST TEST COLLECTIONS WITH INTEGRATION, NOT UNIT TESTS
            var fees = new List<ExciseFee>();
            int length = 5;

            for (int i = 0; i < length; i++)
            {
                fees.Add(ExciseFee.Create(
                    Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - length),
                    ExciseFeeType.Flat,
                    ExciseFee.MinimumValue + length).Value);
            }

            return fees;
        }
    }
}