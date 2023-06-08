using Bogus;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace TestingHelperLibrary.Fakers
{
    public class DriversLicenseFaker : Faker<DriversLicense>
    {
        public DriversLicenseFaker()
        {
            CustomInstantiator(faker =>
            {
                var number = faker.Random.Replace("#########");
                var state = faker.PickRandom<State>();
                var validFrom = faker.Date.Between(new DateTime(2016, 1, 1), new DateTime(2022, 12, 31));
                var validThru = validFrom.AddYears(faker.Random.Int(1, 10));
                DateTimeRange validDateRange = DateTimeRange.Create(
                    validFrom,
                    validThru).Value;

                var result = DriversLicense.Create(number, state, validDateRange);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
