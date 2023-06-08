using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderPurchaseFaker : Faker<RepairOrderPurchase>
    {
        public RepairOrderPurchaseFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var vendor = new VendorFaker(true);
                var purchaseDate = faker.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1));
                var pONumber = $"PO-{faker.Finance.Account(10)}";
                var vendorInvoiceNumber = $"INV-{faker.Finance.Account(10)}";
                var partNumberFormat = "####-###-####";
                var vendorPartNumber = faker.Random.Replace(partNumberFormat);

                var result = RepairOrderPurchase.Create(vendor, purchaseDate, pONumber, vendorInvoiceNumber, vendorPartNumber);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
