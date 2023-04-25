using Bogus;
using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Tests.Helpers.Fakers
{
    public class InventoryItemPartFaker : Faker<InventoryItemPart>
    {
        public InventoryItemPartFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var result = TechAmount.Create(ItemLaborType.Flat, 99.99, SkillLevel.A)
                    .Bind(techAmount => InventoryItemPart.Create(
                        1.1, 2.2, 1.1, 4.4, techAmount, false))
                    .OnFailure(error => throw new InvalidOperationException($"Failed to create TechAmount: {error}"))
                    .Result;

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}