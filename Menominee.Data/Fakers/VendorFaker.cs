using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace Menominee.Data.Fakers
{
    public class VendorFaker
    {
        readonly static int emailsToCreateCount = 3;
        readonly static int phonesToCreateCount = 3;
        public static List<Vendor> MakeVendorFakes(int vendorsToGenerateCount)
        {
            //Set the randomizer seed to generate repeatable data sets.
            //Randomizer.Seed = new Random(549);
            int retries = 10;
            bool success = false;

            while (!success && retries > 0)
            {
                try
                {
                    return new Faker<Vendor>()

                        .CustomInstantiator(faker =>
                        {
                            return Vendor.Create(
                                name: faker.Company.CompanyName(),
                                vendorCode: faker.Random.AlphaNumeric(10),
                                vendorRole: faker.PickRandom<VendorRole>(),
                                notes: faker.Random.Words(25),
                                address: FakeAddress.Generate(),
                                emails: MakeEmailFakes(),
                                phones: MakePhoneFakes()
                            ).Value;

                        }).Generate(vendorsToGenerateCount);
                }
                catch (Exception)
                {
                    retries--;
                }
            }

            return new List<Vendor>();
        }

        public static List<VendorInvoicePaymentMethod> MakePaymentMethodFakes(IReadOnlyList<string> paymentMethodNames, int count)
        {
            int retries = 10;
            bool success = false;

            while (!success && retries > 0)
            {
                try
                {
                    return new Faker<VendorInvoicePaymentMethod>()

                    .CustomInstantiator(faker =>
                        VendorInvoicePaymentMethod.Create(
                            paymentMethodNames,
                            faker.Finance.TransactionType(),
                            isActive: true,
                            faker.PickRandom<VendorInvoicePaymentMethodType>(),
                            reconcilingVendor: null)
                        .Value)
                    .Generate(count);
                }
                catch (CSharpFunctionalExtensions.ResultFailureException)
                {
                    retries--;
                }
            }

            return new List<VendorInvoicePaymentMethod>();
        }

        public static List<DefaultPaymentMethod> MakeDefaultPaymentMethodFakes(VendorInvoicePaymentMethod paymentMethod, int count)
        {
            return new Faker<DefaultPaymentMethod>()
                .CustomInstantiator(faker =>
                    DefaultPaymentMethod.Create(
                        paymentMethod,
                        autoCompleteDocuments: faker.Random.Bool())
                    .Value)
                .Generate(count);
        }

        public static Faker<Address> FakeAddress { get; set; } = new Faker<Address>()
            .CustomInstantiator(faker =>
                Address.Create(
                    faker.Address.StreetAddress(),
                    faker.Address.City(),
                    faker.PickRandom<State>(),
                    faker.Address.ZipCode())
                .Value);

        public static List<Email> MakeEmailFakes()
        {
            return new Faker<Email>()
                    .CustomInstantiator(faker =>
                        Email.Create(
                            faker.Internet.Email(),
                            // Create all emails as non-primary
                            // Using faker.Random.Bool() could
                            // result in >1 primary
                            false)
                        .Value)
                    .Generate(emailsToCreateCount);
        }

        public static List<Phone> MakePhoneFakes()
        {
            return new Faker<Phone>()
                    .CustomInstantiator(faker =>
                        Phone.Create(
                            faker.Phone.PhoneNumber(),
                            faker.PickRandom<PhoneType>(),
                            // Create all emails as non-primary
                            // Using faker.Random.Bool() could
                            // result in >1 primary
                            false)
                        .Value)
                    .Generate(phonesToCreateCount);
        }
    }
}
