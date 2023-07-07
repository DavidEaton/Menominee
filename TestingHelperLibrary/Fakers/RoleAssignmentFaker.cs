using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RoleAssignmentFaker : Faker<RoleAssignment>
    {
        public RoleAssignmentFaker(bool generateId, EmploymentRole? employmentRole = null)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var person = new PersonFaker(true).Generate();
                var role = employmentRole ?? EmploymentRole.Technician;
                var result = RoleAssignment.Create(
                    role);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
