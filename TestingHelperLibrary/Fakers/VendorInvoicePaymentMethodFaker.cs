using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoicePaymentMethodFaker : Faker<VendorInvoicePaymentMethod>
    {
        private readonly List<string> remainingPaymentMethodNames;
        public VendorInvoicePaymentMethodFaker(bool generateId)
        {
            remainingPaymentMethodNames = new List<string> { "Credit Card", "Cash", "Debit Card", "Bank Transfer", "Check", "PayPal" };

            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                if (remainingPaymentMethodNames.Count == 0)
                    //Stop creating when name list runs out");
                    return null;

                var name = faker.PickRandom(remainingPaymentMethodNames);
                remainingPaymentMethodNames.Remove(name); var isActive = true;

                var paymentMethodMethods = new List<string>();
                var paymentMethodType = faker.PickRandom<VendorInvoicePaymentMethodType>();
                var reconcilingVendor = faker.Random.Bool() ? new VendorFaker(generateId).Generate() : null;

                var result = VendorInvoicePaymentMethod.Create(paymentMethodMethods, name, isActive, paymentMethodType, reconcilingVendor);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
