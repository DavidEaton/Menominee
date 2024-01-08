using Bogus;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;

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

                var result = Address.Create(
                    streetAddress,
                    city,
                    state,
                    "55555");

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
