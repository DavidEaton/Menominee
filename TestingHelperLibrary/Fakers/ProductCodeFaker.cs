using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Extensions;
using TestingHelperLibrary.Fakers;

namespace CustomerVehicleManagement.Tests.Helpers.Fakers
{
    public class ProductCodeFaker : Faker<ProductCode>
    {
        public ProductCodeFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var saleCode = new SaleCodeFaker(generateId).Generate(); //optional
                var manufacturer = new ManufacturerFaker(generateId).Generate();
                var manufacturers = new List<string>();
                var code = faker.Commerce.Ean13().Truncate(10);
                var name = faker.Commerce.ProductName().Truncate(255);

                var result = ProductCode.Create(manufacturer, code, name, manufacturers, saleCode);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}