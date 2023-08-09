using Bogus;
using Menominee.Domain.Entities;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class PhoneFaker : Faker<Phone>
    {
        public PhoneFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);
            GeneratePhone();
        }

        public PhoneFaker(long id = 0)
        {
            RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);
            GeneratePhone();
        }

        private void GeneratePhone()
        {
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
