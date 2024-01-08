using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Extensions;
using TestingHelperLibrary.Fakers;

namespace Menominee.Tests.Helpers.Fakers
{
    public class ProductCodeFaker : Faker<ProductCode>
    {
        public ProductCodeFaker(
            bool generateId = false,
            SaleCode saleCodeFromCaller = null,
            Manufacturer manufacturerFromCaller = null)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var saleCode = saleCodeFromCaller is not null
                    ? saleCodeFromCaller
                    : new SaleCodeFaker(generateId).Generate(); //optional
                var manufacturer = manufacturerFromCaller is not null
                    ? manufacturerFromCaller
                    : new ManufacturerFaker(generateId).Generate();
                var manufacturers = new List<string>();
                var code = faker.Commerce.Ean13().Truncate(8);
                var name = faker.Commerce.ProductName().Truncate(255);

                var result = ProductCode.Create(manufacturer, code, name, manufacturers, saleCode);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}