using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RoleAssignmentFaker : Faker<RoleAssignment>
    {
        public RoleAssignmentFaker(bool generateId, EmploymentRole? employmentRole = null, RoleAssignmentBehavior behavior = RoleAssignmentBehavior.Default)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var role = EmploymentRole.Technician;
                switch (behavior)
                {
                    case RoleAssignmentBehavior.Random:
                        role = faker.PickRandom<EmploymentRole>();
                        break;
                    case RoleAssignmentBehavior.Specific:
                        role = employmentRole ?? EmploymentRole.Technician;
                        break;
                    case RoleAssignmentBehavior.Default:
                        role = EmploymentRole.Technician;
                        break;
                }

                var result = RoleAssignment.Create(
                    role);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
    public enum RoleAssignmentBehavior
    {
        Random,
        Specific,
        Default
    }
}
