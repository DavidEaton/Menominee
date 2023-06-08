using Bogus;
using CustomerVehicleManagement.Data.Database;
using CustomerVehicleManagement.Data.Fakers;
using CustomerVehicleManagement.Data.Results;
using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Data.Generators
{
    public static class VendorGenerator
    {
        public static void GenerateData()
        {
            Helper.ClearDatabase();
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
            // Randomly assign to some Vendors: DefaultPaymentMethod
            Random random = new();
            foreach (var vendor in vendors)
                if (faker.Random.Bool())
                {
                    random = new Random();
                    vendor.SetDefaultPaymentMethod(defaultPaymentMethods[faker.Random.Int(0, defaultPaymentMethods.Count - 1)]);
                }

            // Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor
            foreach (var paymentMethod in paymentMethods)
            {
                random = new();
                paymentMethod.SetReconcilingVendor(vendors[random.Next(0, CountOfPaymentMethodsToCreate - 1)]);
            }

            // Assign IsPrimary to first Phone in Vendor.Phones collection
            foreach (var vendor in vendors)
                vendor.Phones[0].SetIsPrimary(true);

            // Assign IsPrimary to first Email in Vendor.Emails collection
            foreach (var vendor in vendors)
                vendor.Emails[0].SetIsPrimary(true);
            return random;
        }
    }
}
