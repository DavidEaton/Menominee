using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderFaker : Faker<RepairOrder>
    {
        public RepairOrderFaker(bool generateId = false, int statusesCount = 0, int servicesCount = 0, int taxesCount = 0, int paymentsCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var accountingDate = faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
                var repairOrderNumbers = new List<long>();
                var lastInvoiceNumber = faker.Random.Long(1000, 100000);
                var statuses = new RepairOrderStatusFaker(generateId: generateId).Generate(statusesCount);
                var services = new RepairOrderServiceFaker(generateId: generateId).Generate(servicesCount);
                var payments = new RepairOrderPaymentFaker(generateId: generateId).Generate(paymentsCount);
                var taxes = new RepairOrderTaxFaker(generateId: generateId).Generate(taxesCount);

                var result = RepairOrder.Create(
                    new CustomerFaker(),
                    new VehicleFaker(),
                    accountingDate,
                    repairOrderNumbers,
                    lastInvoiceNumber,
                    statuses,
                    services,
                    taxes,
                    payments);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
