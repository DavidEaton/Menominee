using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderPaymentFaker : Faker<RepairOrderPayment>
    {
        public RepairOrderPaymentFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var paymentMethod = faker.PickRandom<PaymentMethod>();
                var amount = Math.Round(faker.Random.Double(0, 100), 2);

                var result = RepairOrderPayment.Create(paymentMethod, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
