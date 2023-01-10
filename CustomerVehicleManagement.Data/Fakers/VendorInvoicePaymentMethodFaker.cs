using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Data.Fakers
{
    public static class VendorInvoicePaymentMethodFaker
    {
        public static VendorInvoicePaymentMethod PaymentMethod { get; set; }

        public static VendorInvoicePaymentMethod GetPaymentMethod()
        {
            return PaymentMethod;
        }

        public static void SetPaymentMethod(VendorInvoicePaymentMethod value)
        {
            PaymentMethod = value;
        }
        public static void MakeFakeVendorInvoicePaymentMethod()
        {
            Randomizer.Seed = new Random(549);

            var faker = new Faker();
            var paymentMethodNames = faker.Make(10, () => faker.Finance.TransactionType());

            var fakeVendorInvoicePaymentMethod = new Faker<VendorInvoicePaymentMethod>()
               .StrictMode(true)

               //Optional: Call for objects that have complex initialization
               .CustomInstantiator(faker => VendorInvoicePaymentMethod.Create(
                   paymentMethodNames,
                   faker.Finance.TransactionType(),
                   true,
                   faker.PickRandom<VendorInvoicePaymentMethodType>(),
                   null).Value);

            fakeVendorInvoicePaymentMethod
                .RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.Name, (faker) => faker.Finance.TransactionType())
               .RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.PaymentType, faker => faker.PickRandom<VendorInvoicePaymentMethodType>())
               .RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.IsActive, true);

            var fakeDefaultPaymentMethod = new Faker<DefaultPaymentMethod>()
                .StrictMode(true)
                .RuleFor(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod, fakeVendorInvoicePaymentMethod)
                .RuleFor(defaultPaymentMethod => defaultPaymentMethod.AutoCompleteDocuments, faker => faker.PickRandom<bool>());

            PaymentMethod = fakeVendorInvoicePaymentMethod.Generate();
        }
    }
}
