using Bogus;
using Menominee.Domain.Entities.RepairOrders;

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

                var statuses = statusesCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(statusesCount)
                            .Select(id => new RepairOrderStatusFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new RepairOrderStatusFaker(generateId: false).Generate(statusesCount);

                var services = servicesCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(servicesCount)
                            .Select(id => new RepairOrderServiceFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new RepairOrderServiceFaker(generateId: false).Generate(servicesCount);

                var payments = paymentsCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(paymentsCount)
                            .Select(id => new RepairOrderPaymentFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new RepairOrderPaymentFaker(generateId: false).Generate(paymentsCount);

                var taxes = taxesCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(taxesCount)
                            .Select(id => new RepairOrderTaxFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new RepairOrderTaxFaker(generateId: false).Generate(taxesCount);

                var result = RepairOrder.Create(
                    new CustomerFaker().Generate(),
                    new VehicleFaker().Generate(),
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
