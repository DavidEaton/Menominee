using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class VendorFaker : Faker<Vendor>
    {
        public VendorFaker(bool generateId, int emailsCount = 0, int phonesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var name = faker.Company.CompanyName();
                var vendorCode = faker.Random.AlphaNumeric(6).ToUpper();
                var vendorRole = faker.PickRandom<VendorRole>();
                var note = faker.Lorem.Sentence(20);
                var defaultPaymentMethod = new DefaultPaymentMethodFaker(generateId).Generate();
                var address = new AddressFaker().Generate();
                var emails = new EmailFaker(generateId).Generate(emailsCount);
                var phones = new PhoneFaker(generateId).Generate(phonesCount);

                var result = Vendor.Create(name, vendorCode, vendorRole, note, defaultPaymentMethod, address, emails, phones);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
