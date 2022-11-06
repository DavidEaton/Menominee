using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public static class VendorInvoiceTestHelper
    {
        public static Vendor CreateVendor()
        {
            return Vendor.Create(
                Utilities.RandomCharacters(Vendor.MinimumLength),
                Utilities.RandomCharacters(Vendor.MinimumLength)).Value;
        }

        public static IList<VendorInvoicePaymentMethodToRead> CreateVendorInvoicePaymentMethods()
        {
            return new List<VendorInvoicePaymentMethodToRead>
            {
                new VendorInvoicePaymentMethodToRead()
                {
                    Id = 1,
                    Name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength),
                    IsActive = true,
                    IsOnAccountPaymentType = false,
                },

                new VendorInvoicePaymentMethodToRead()
                {
                    Id = 1,
                    Name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 10),
                    IsActive = true,
                    IsOnAccountPaymentType = false,
                },

                new VendorInvoicePaymentMethodToRead()
                {
                    Id = 1,
                    Name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 20),
                    IsActive = true,
                    IsOnAccountPaymentType = false,
                }
            };
        }

        public static VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            var paymentMethodNames = CreatePaymentMethodNames();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor).Value;
        }

        public static VendorInvoicePayment CreateVendorInvoicePayment()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            double amount = VendorInvoicePayment.InvalidValue + 1;
            return VendorInvoicePayment.Create(paymentMethod, amount).Value;
        }


        public static SalesTax CreateSalesTax(int dcescriptionSeed = 0)
        {
            var description = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;
            bool? isAppliedByDefault = true;
            bool? isTaxable = true;

            return SalesTax.Create(description, taxType, order, taxIdNumber, partTaxRate, laborTaxRate, isAppliedByDefault: isAppliedByDefault, isTaxable: isTaxable).Value;
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

        internal static IList<string> CreatePaymentMethodNames()
        {
            IList<string> result = new List<string>();
            var list = CreateVendorInvoicePaymentMethods();

            foreach (var method in list)
            {
                result.Add(method.Name);
            }

            return result;
        }
    }
}