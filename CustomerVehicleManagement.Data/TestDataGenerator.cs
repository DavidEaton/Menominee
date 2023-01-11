using Bogus;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Data.Fakers;
using Microsoft.EntityFrameworkCore;

namespace CustomerVehicleManagement.Data
{
    public static class TestDataGenerator
    {
        // TODO: Move IntegrationTestsConnectionString to settings/configuration
        internal const string MenomineeConnectionString = @"Server=localhost;Database=Menominee;Trusted_Connection=True;";

        public static void GenerateData()
        {
            ClearDatabase();
            Generate();
            SaveToDatabase();
        }

        private static void SaveToDatabase()
        {
            ApplicationDbContext context = CreateTestContext();

            context.Vendors.AddRange(TestDataGeneratorResults.Vendors);
            context.SaveChanges();
        }

        internal static ApplicationDbContext CreateTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(MenomineeConnectionString);

            return new ApplicationDbContext(MenomineeConnectionString);
        }

        private static void ClearDatabase()
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
        }

        private static void Generate()
        {
            var random = new Random();
            var CountOfVendorsToCreate = 25;
            var CountOfPaymentMethodsToCreate = CountOfVendorsToCreate;

            var faker = new Faker();
            var vendors = VendorFaker.MakeVendorFakes(CountOfVendorsToCreate);
            var paymentMethodNames = faker.Make(CountOfVendorsToCreate / 2, () => faker.Random.Words());
            var paymentMethods = VendorFaker.MakePaymentMethodFakes(
                paymentMethodNames: paymentMethodNames,
                count: CountOfPaymentMethodsToCreate);
            var defaultPaymentMethods = VendorFaker.MakeDefaultPaymentMethodFakes(
                paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
                count: CountOfPaymentMethodsToCreate / 2);
            defaultPaymentMethods.AddRange(VendorFaker.MakeDefaultPaymentMethodFakes(
                paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
                count: CountOfPaymentMethodsToCreate / 2));

            // Randomly assign to some Vendors: DefaultPaymentMethod
            random = new Random();
            foreach (var vendor in vendors)
                if (faker.Random.Bool())
                {
                    random = new Random();
                    vendor.SetDefaultPaymentMethod(defaultPaymentMethods[faker.Random.Int(0, defaultPaymentMethods.Count - 1)]);
                }

            // TODO: Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor
            foreach (var paymentMethod in paymentMethods)
            {
                random = new Random();
                paymentMethod.SetReconcilingVendor(vendors[random.Next(0, CountOfPaymentMethodsToCreate - 1)]);
            }

            // TODO: Assign IsPrimary to first Phone in Vendor.Phones collection
            foreach (var vendor in vendors)
                vendor.Phones[0].SetIsPrimary(true);

            // TODO: Assign IsPrimary to first Email in Vendor.Emails collection
            foreach (var vendor in vendors)
                vendor.Emails[0].SetIsPrimary(true);

            foreach (var vendor in vendors)
            {
                Console.WriteLine($"Vendor Name: {vendor.Name}");
                Console.WriteLine($"DefaultPaymentMethod Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.Name}");
                Console.WriteLine($"DefaultPaymentMethod ReconcilingVendor Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.ReconcilingVendor.Name}");
            }

            // Make those created collections of test data part of this app's public api
            TestDataGeneratorResults.DefaultPaymentMethods = defaultPaymentMethods;
            TestDataGeneratorResults.PaymentMethods = paymentMethods;
            TestDataGeneratorResults.Vendors = vendors;

            // Add those collections to the ApplicationDbContext and save to database


            Console.ReadLine();
        }

    }
}
