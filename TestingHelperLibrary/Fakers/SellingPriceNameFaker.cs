using Bogus;
using Menominee.Domain.Entities.Inventory;

namespace Menominee.TestingHelperLibrary.Fakers;

public class SellingPriceNameFaker : Faker<SellingPriceName>
{
    public SellingPriceNameFaker(bool generateId = false)
    {
        RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);
        GenerateSellingPriceName();
    }

    public SellingPriceNameFaker(long id = 0)
    {
        RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);
        GenerateSellingPriceName();
    }

    private void GenerateSellingPriceName()
    {
        CustomInstantiator(faker =>
        {
            var name = faker.Random.String2(SellingPriceName.MinimumNameLength, SellingPriceName.MaximumNameLength);

            var result = SellingPriceName.Create(name);

            return result.IsSuccess
                ? result.Value
                : throw new InvalidOperationException(result.Error);
        });
    }
}
