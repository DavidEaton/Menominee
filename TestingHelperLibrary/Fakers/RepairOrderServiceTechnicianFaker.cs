using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderServiceTechnicianFaker : Faker<RepairOrderServiceTechnician>
    {
        public RepairOrderServiceTechnicianFaker(bool generateId = false, long id = 0, List<Employee> employees = null)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                Employee employee;

                if (employees is not null && employees.Count > 0)
                    employee = faker.PickRandom(employees);
                else
                    employee = Employee.Create(new PersonFaker(generateId).Generate(), new List<RoleAssignment>()).Value;

                var result = RepairOrderServiceTechnician.Create(employee);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
