﻿using Bogus;
using Menominee.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderItemTaxFaker : Faker<RepairOrderItemTax>
    {
        public RepairOrderItemTaxFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
                var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

                var result = RepairOrderItemTax.Create(
                    PartTax.Create(rate, amount).Value,
                    LaborTax.Create(rate, amount).Value);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
