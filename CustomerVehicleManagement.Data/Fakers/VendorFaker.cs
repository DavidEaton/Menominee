﻿using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Data.Fakers
{
    public class VendorFaker
    {
        public static List<Vendor> MakeVendorFakes(int vendorsToGenerateCount)
        {
            //Set the randomizer seed to generate repeatable data sets.
            //Randomizer.Seed = new Random(549);

            var faker = new Faker();

            // TODO: use fakeDefaultPaymentMethod randomly; not all Vendors have DefaultPaymentMethod.
            var fakeVendor = new Faker<Vendor>()
                .CustomInstantiator(faker =>
                {
                    var companyName = faker.Company.CompanyName();
                    var vendorRole = faker.PickRandom<VendorRole>();
                    var vendorCode = faker.Random.AlphaNumeric(10);
                    var vendorNote = faker.Random.Words(25);

                    return Vendor.Create(companyName, vendorCode, vendorRole, vendorNote).Value;
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
    }
}
