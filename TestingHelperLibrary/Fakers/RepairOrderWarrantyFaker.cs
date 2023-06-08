using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderWarrantyFaker : Faker<RepairOrderWarranty>
    {
        public RepairOrderWarrantyFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var quantity = (double)Math.Round(faker.Random.Decimal(1, 22), 0);
                var type = faker.PickRandom<WarrantyType>();
                var newWarranty = faker.Company.CompanyName();
                var originalWarranty = faker.Company.CompanyName();
                var originalInvoiceId = faker.Random.Long(1, 22);

                var result = RepairOrderWarranty.Create(quantity, type, newWarranty, originalWarranty, originalInvoiceId);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
