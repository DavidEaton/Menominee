using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using System;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceItemFaker : Faker<VendorInvoiceItem>
    {
        public VendorInvoiceItemFaker(bool generateId)
        {
            CustomInstantiator(faker =>
                {
                    var partNumber = $"{faker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}-{faker.Random.Number(1000, 9999)}";
                    var description = faker.Random.String2(VendorInvoiceItem.MinimumLength, VendorInvoiceItem.MaximumLength);
                    var manufacturer = new ManufacturerFaker(generateId).Generate();
                    var saleCode = new SaleCodeFaker(generateId).Generate();

                    var result = VendorInvoiceItem.Create(partNumber, description, manufacturer, saleCode);

                    return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
                });
        }
    }
}
