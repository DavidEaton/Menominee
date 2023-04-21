using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class VendorFaker : Faker<Vendor>
    {
        public VendorFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var name = faker.Company.CompanyName();
                var vendorCode = faker.Random.AlphaNumeric(6).ToUpper();
                var vendorRole = faker.PickRandom<VendorRole>();
                var note = faker.Lorem.Sentence(20);
                var defaultPaymentMethod = new DefaultPaymentMethodFaker(generateId).Generate();
                //var address = new AutoFaker<Address>().Generate();
                //var emails = new AutoFaker<Email>().Generate(2);
                //var phones = new AutoFaker<Phone>().Generate(2);

                var result = Vendor.Create(name, vendorCode, vendorRole, note, defaultPaymentMethod);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
