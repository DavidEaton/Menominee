using Bogus;
using CustomerVehicleManagement.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class EmployeeFaker : Faker<Employee>
    {
        public EmployeeFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var person = new PersonFaker(true).Generate();
                var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
                var hired = DateTime.Today;
                var notes = faker.Random.String2(1, 500);
                var result = Employee.Create(
                    person,
                    roleAssignments,
                    hired,
                    notes);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
