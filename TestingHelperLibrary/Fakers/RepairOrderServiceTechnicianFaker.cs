using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderServiceTechnicianFaker : Faker<RepairOrderServiceTechnician>
    {
        public RepairOrderServiceTechnicianFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var employee = Employee.Create(new PersonFaker(generateId).Generate(), new List<RoleAssignment>()).Value;
                var result = RepairOrderServiceTechnician.Create(employee);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
