using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using TestingHelperLibrary.Fakers;

namespace CustomerVehicleManagement.Tests.Helpers.Fakers
{
    public class InventoryItemFaker : Faker<InventoryItem>
    {
        public InventoryItemFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var manufacturer = new ManufacturerFaker(generateId).Generate();
                var itemNumber = faker.Random.AlphaNumeric(10).ToUpper();
                var name = faker.Commerce.ProductName().Truncate(255);
                var description = faker.Commerce.ProductDescription().Truncate(255);
                var productCode = new ProductCodeFaker(generateId).Generate();
                var part = new InventoryItemPartFaker(generateId).Generate();

                var result = InventoryItem.Create(manufacturer, itemNumber, description, productCode, InventoryItemType.Part, part);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}