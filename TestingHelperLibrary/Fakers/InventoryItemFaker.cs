using Bogus;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Enums;
using Menominee.Domain.Extensions;
using TestingHelperLibrary.Fakers;

namespace Menominee.Tests.Helpers.Fakers
{
    public class InventoryItemFaker : Faker<InventoryItem>
    {
        public InventoryItemFaker(bool generateId,
            int collectionCount = 0,
            Manufacturer manufacturerFromCaller = null,
            ProductCode productCodeFromCaller = null,
            InventoryItemPart partFromCaller = null)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var manufacturer = manufacturerFromCaller is null
                    ? new ManufacturerFaker(generateId).Generate()
                    : manufacturerFromCaller;
                var itemNumber = faker.Random.AlphaNumeric(10).ToUpper();
                var name = faker.Commerce.ProductName().Truncate(255);
                var description = faker.Commerce.ProductDescription().Truncate(255);
                var productCode = productCodeFromCaller is null
                    ? new ProductCodeFaker(generateId).Generate()
                    : productCodeFromCaller;
                var part = partFromCaller is null
                    ? new InventoryItemPartFaker(generateId, collectionCount).Generate()
                    : partFromCaller;

                var result = InventoryItem.Create(manufacturer, itemNumber, description, productCode, InventoryItemType.Part, part);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}