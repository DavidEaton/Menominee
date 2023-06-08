using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class PhoneFaker : Faker<Phone>
    {
        public PhoneFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var phoneType = faker.PickRandom<PhoneType>();
                var number = faker.Phone.PhoneNumber("##########");
                var isPrimary = false;
                var result = Phone.Create(number, phoneType, isPrimary);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
