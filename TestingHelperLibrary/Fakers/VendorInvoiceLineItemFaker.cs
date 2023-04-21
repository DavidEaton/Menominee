using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceLineItemFaker : Faker<VendorInvoiceLineItem>
    {
        public VendorInvoiceLineItemFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var type = faker.PickRandom<VendorInvoiceLineItemType>();
                var quantity = faker.Random.Double(1, 25);
                var cost = Math.Round(faker.Random.Double(1, 1000), 2);
                var core = Math.Round(faker.Random.Double(1, 1000), 2);
                var poNumber = $"{faker.Random.AlphaNumeric(2).ToUpper()}-{faker.Random.Number(1000, 9999)}";
                var item = new VendorInvoiceItemFaker(generateId).Generate();
                var result = VendorInvoiceLineItem.Create(type, item, quantity, cost, core, poNumber);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
