using Bogus;
using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;

namespace Menominee.Tests.Helpers.Fakers
{
    public class InventoryItemPartFaker : Faker<InventoryItemPart>
    {
        public InventoryItemPartFaker(bool generateId, int exciseFeeCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var result = TechAmount.Create(ItemLaborType.Flat, 99.99, SkillLevel.A)
                    .Bind(techAmount => InventoryItemPart.Create(
                        1.1, 2.2, 1.1, 4.4, techAmount, false))
                    .OnFailure(error => throw new InvalidOperationException($"Failed to create TechAmount: {error}"))
                    .Result;

                var exciseFees = exciseFeeCount <= 0
                    ? new()
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(exciseFeeCount)
                            .Select(id => new ExciseFeeFaker(id: id).Generate())
                            .ToList()
                        : new ExciseFeeFaker(generateId: false).Generate(exciseFeeCount);

                var combinedAddResult = Result.Combine(exciseFees
                    .Select(fee => result.Value.AddExciseFee(fee))
                    .ToList());

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}