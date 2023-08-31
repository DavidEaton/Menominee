using Bogus;
using Menominee.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class EmployeeFaker : Faker<Employee>
    {
        public EmployeeFaker(bool generateId = false, int collectionCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var person = new PersonFaker(generateId).Generate();
                var hired = DateTime.Today;
                var notes = faker.Random.String2(1, 500);
                var result = Employee.Create(
                    person,
                    new List<RoleAssignment>(),
                    hired,
                    notes);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
