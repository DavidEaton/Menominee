using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;

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
                var ssn = faker.Random.String2(1, 12);
                var certificationNumber = faker.Random.String2(1, 20);
                var active = faker.Random.Bool();
                var printedName = faker.Random.String2(1, 50);
                var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
                var benefitLoad = faker.Random.Double(0.0, 100.0);

                var result = Employee.Create(
                    person,
                    new List<RoleAssignment>(),
                    hired,
                    notes,
                    ssn,
                    certificationNumber,
                    active,
                    printedName,
                    expenseCategory,
                    benefitLoad);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
