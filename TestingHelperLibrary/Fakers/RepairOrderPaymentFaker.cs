using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderPaymentFaker : Faker<RepairOrderPayment>
    {
        public RepairOrderPaymentFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var paymentType = faker.PickRandom<PaymentMethod>();
                var amount = faker.Random.Double(0, 100);

                var result = RepairOrderPayment.Create(paymentType, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
