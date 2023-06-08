using Bogus;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace TestingHelperLibrary.Fakers
{
    public class AddressFaker : Faker<Address>
    {
        public AddressFaker()
        {
            CustomInstantiator(faker =>
            {
                var streetAddress = faker.Address.StreetAddress();
                var city = faker.Address.City();
                var state = faker.PickRandom<State>();
                var zipCode = faker.Address.ZipCode();

                var result = Address.Create(
                    streetAddress,
                    city,
                    state,
                    zipCode);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
