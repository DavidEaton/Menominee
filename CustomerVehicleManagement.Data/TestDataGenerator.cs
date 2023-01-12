using Bogus;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Data.Fakers;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CustomerVehicleManagement.Data
{
    public static class TestDataGenerator
    {
        // TODO: Move IntegrationTestsConnectionString to settings/configuration
        internal const string MenomineeConnectionString = @"Server=localhost;Database=Menominee;Trusted_Connection=True;";
        static int savedVendors = 0;

        public static bool EnsureDeletedEnsureCreated { get; private set; } = true;

        public static void GenerateData()
        {
            ClearDatabase();
            Generate();

            foreach (var vendor in TestDataGeneratorResults.Vendors)
                SaveToDatabase(vendor);

            Console.ReadLine();
        }

        private static void SaveToDatabase(Vendor vendor)
        {
            using (var context = new ApplicationDbContext(MenomineeConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                    context.Database.OpenConnection();

                try
                {
                    context.Vendors.Add(vendor);
                    context.SaveChanges();
                    savedVendors++;
                }
                catch (Exception ex)
                {
                    // Continue after failed insert
                    Console.WriteLine($"failed insert: {ex}");
                    Console.WriteLine();
                    Console.WriteLine("Continuing with retry...");
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"Saved {savedVendors} rows!");
        }

        private static void ClearDatabase()
        {
            using (var context = new ApplicationDbContext(MenomineeConnectionString))
            {
                if (context.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    try
                    {
                        // Will fail if database does not exist
                        context.Database.OpenConnection();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"failed ClearDatabase(): {ex}");

                        Console.WriteLine();
                        Console.WriteLine($"Creating database...");
                        Console.WriteLine();
                        context.Database.EnsureCreated();
                        EnsureDeletedEnsureCreated = false;
                    }
                }

                try
                {
                    if (EnsureDeletedEnsureCreated)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Deleting database...");
                        Console.WriteLine();
                        context.Database.EnsureDeleted();

                        Console.WriteLine();
                        Console.WriteLine($"Creating database...");
                        Console.WriteLine();
                        context.Database.EnsureCreated();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"failed ClearDatabase(): {ex}");
                    Console.WriteLine();
                }
            }
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
            TestDataGeneratorResults.DefaultPaymentMethods = defaultPaymentMethods;
            TestDataGeneratorResults.PaymentMethods = paymentMethods;
            TestDataGeneratorResults.Vendors = vendors;
        }

        private static Random MakeRandomAssignments(int CountOfPaymentMethodsToCreate, Faker faker, List<Vendor> vendors, List<VendorInvoicePaymentMethod> paymentMethods, List<DefaultPaymentMethod> defaultPaymentMethods)
        {
            // Randomly assign to some Vendors: DefaultPaymentMethod
            Random random = new Random();
            foreach (var vendor in vendors)
                if (faker.Random.Bool())
                {
                    random = new Random();
                    vendor.SetDefaultPaymentMethod(defaultPaymentMethods[faker.Random.Int(0, defaultPaymentMethods.Count - 1)]);
                }

            // Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor
            foreach (var paymentMethod in paymentMethods)
            {
                random = new Random();
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
