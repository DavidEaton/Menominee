using Bogus;
using Menominee.Data.Database;
using Menominee.Data.Results;
using Menominee.Domain.Entities.Payables;
using Menominee.TestingHelperLibrary.Fakers;

namespace Menominee.Data.Generators
{
    public static class VendorGenerator
    {
        public static void GenerateData()
        {
            GenerateVendors();

            foreach (var vendor in VendorGeneratorResult.Vendors)
                Helper.SaveToDatabase(vendor);
        }

        private static void GenerateVendors()
        {
            // TODO: Move to appSetting, make settable at runtime
            var CountOfVendorsToCreate = 25;
            var CountOfPaymentMethodsToCreate = CountOfVendorsToCreate;
            var random = new Random();

            var faker = new Faker();
            var vendors = VendorFaker.MakeVendorFakes(CountOfVendorsToCreate);
            var paymentMethodNames = faker.Make(CountOfVendorsToCreate / 2, () => faker.Random.Words());
            var paymentMethods = VendorFaker.MakePaymentMethodFakes(
                paymentMethodNames: (IReadOnlyList<string>)paymentMethodNames,
                count: CountOfPaymentMethodsToCreate);

            List<DefaultPaymentMethod> defaultPaymentMethods = new();
            if (paymentMethods.Count > 0)
            {
                defaultPaymentMethods = VendorFaker.MakeDefaultPaymentMethodFakes(
                    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
                    count: CountOfPaymentMethodsToCreate / 2);

                defaultPaymentMethods.AddRange(VendorFaker.MakeDefaultPaymentMethodFakes(
                    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
                    count: CountOfPaymentMethodsToCreate / 2));

                random = MakeRandomAssignments(CountOfPaymentMethodsToCreate, faker, vendors, paymentMethods, defaultPaymentMethods);
            }

            // Make those created collections of test data part of this app's public api
            VendorGeneratorResult.DefaultPaymentMethods = defaultPaymentMethods;
            VendorGeneratorResult.PaymentMethods = paymentMethods;
            VendorGeneratorResult.Vendors = vendors;
        }

        private static Random MakeRandomAssignments(int CountOfPaymentMethodsToCreate, Faker faker, List<Vendor> vendors, List<VendorInvoicePaymentMethod> paymentMethods, List<DefaultPaymentMethod> defaultPaymentMethods)
        {
            // Use a single instance of Random for better performance and randomness.
            Random random = new Random();

            // Randomly assign to some Vendors: DefaultPaymentMethod
            foreach (var vendor in vendors)
            {
                if (faker.Random.Bool() && defaultPaymentMethods.Count > 0)
                {
                    var randomPaymentMethodIndex = random.Next(defaultPaymentMethods.Count);
                    vendor.SetDefaultPaymentMethod(defaultPaymentMethods[randomPaymentMethodIndex]);
                }
            }

            // Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor
            if (vendors.Count > 0) // Ensure there is at least one vendor.
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    var randomVendorIndex = random.Next(vendors.Count); // Use vendors.Count instead.
                    paymentMethod.SetReconcilingVendor(vendors[randomVendorIndex]);
                }
            }

            // Check if vendors have any Phones or Emails before assigning IsPrimary.
            foreach (var vendor in vendors)
            {
                if (vendor.Phones.Count > 0)
                {
                    vendor.Phones[0].SetIsPrimary(true);
                }

                if (vendor.Emails.Count > 0)
                {
                    vendor.Emails[0].SetIsPrimary(true);
                }
            }

            return random;
        }

    }
}
