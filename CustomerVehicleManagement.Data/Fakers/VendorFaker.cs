using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Data.Fakers
{
    public class VendorFaker
    {
        public static List<Vendor> MakeVendorFakes(int vendorsToGenerateCount)
        {
            //Set the randomizer seed to generate repeatable data sets.
            //Randomizer.Seed = new Random(549);

            var fakeVendor = new Faker<Vendor>()

                .CustomInstantiator(faker =>
                {
                    var companyName = faker.Company.CompanyName();
                    var vendorRole = faker.PickRandom<VendorRole>();
                    var vendorCode = faker.Random.AlphaNumeric(10);
                    var vendorNote = faker.Random.Words(25);

                    return Vendor.Create(
                        companyName,
                        vendorCode,
                        vendorRole,
                        vendorNote,
                        address: FakeAddress.Generate(),
                        emails: MakeEmailFakes(),
                        phones: MakePhoneFakes()
                    ).Value;
                });

            return fakeVendor.Generate(vendorsToGenerateCount);
        }

        public static List<VendorInvoicePaymentMethod> MakePaymentMethodFakes(IList<string> paymentMethodNames, int count)
        {
            var fakeVendorInvoicePaymentMethod = new Faker<VendorInvoicePaymentMethod>()

               .CustomInstantiator(faker => VendorInvoicePaymentMethod.Create(
                   paymentMethodNames,
                   faker.Finance.TransactionType(),
                   true,
                   faker.PickRandom<VendorInvoicePaymentMethodType>(),
                   null).Value);

            return fakeVendorInvoicePaymentMethod.Generate(count);
        }

        public static List<DefaultPaymentMethod> MakeDefaultPaymentMethodFakes(VendorInvoicePaymentMethod paymentMethod, int count)
        {
            var fakeDefaultPaymentMethod = new Faker<DefaultPaymentMethod>()
                .CustomInstantiator(faker =>
                    DefaultPaymentMethod.Create(
                        paymentMethod,
                        faker.Random.Bool())
                    .Value);

            return fakeDefaultPaymentMethod.Generate(count);
        }

        public static List<Address> MakeAddressFakes(int count)
        {
            var fakeAddress = new Faker<Address>()
                    .CustomInstantiator(faker =>
                    Address.Create(
                        faker.Address.StreetAddress(),
                        faker.Address.City(),
                        faker.PickRandom<State>(),
                        faker.Address.ZipCode()).Value
                    );

            return fakeAddress.Generate(count);
        }

        public static Faker<Address> FakeAddress { get; set; } = new Faker<Address>()
            .CustomInstantiator(faker =>
                Address.Create(
                    faker.Address.StreetAddress(),
                    faker.Address.City(),
                    faker.PickRandom<State>(),
                    faker.Address.ZipCode()).Value
                );

        public static List<Email> MakeEmailFakes()
        {
            var fakeEmail = new Faker<Email>()
                    .CustomInstantiator(faker =>
                        Email.Create(
                            faker.Internet.Email(),
                            // Create all emails as non-primary
                            // Using faker.Random.Bool() could
                            // result in >1 primary
                            false).Value
                );

            return fakeEmail.Generate(3);
        }

        public static Faker<Email> FakeEmail { get; set; } = new Faker<Email>()
            .CustomInstantiator(faker =>
                Email.Create(
                    faker.Internet.Email(),
                    // Create all emails as non-primary
                    // Using faker.Random.Bool() could
                    // result in >1 primary
                    false).Value
                );


        public static List<Phone> MakePhoneFakes()
        {
            var fakeEmail = new Faker<Phone>()
                    .CustomInstantiator(faker =>
                        Phone.Create(
                            faker.Phone.PhoneNumber(),
                            faker.PickRandom<PhoneType>(),
                            // Create all emails as non-primary
                            // Using faker.Random.Bool() could
                            // result in >1 primary
                            false)
                        .Value);

            return fakeEmail.Generate(3);
        }
        public static Faker<Phone> FakePhone { get; set; } = new Faker<Phone>()
            .CustomInstantiator(faker =>
                Phone.Create(
                    faker.Phone.PhoneNumber(),
                    faker.PickRandom<PhoneType>(),
                    // Create all emails as non-primary
                    // Using faker.Random.Bool() could
                    // result in >1 primary
                    false)
            .Value);
    }
}
