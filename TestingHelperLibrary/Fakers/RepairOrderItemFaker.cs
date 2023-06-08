using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Tests.Helpers.Fakers;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderItemFaker : Faker<RepairOrderItem>
    {
        public RepairOrderItemFaker()
        {
            CustomInstantiator(faker =>
            {
                var manufacturer = new ManufacturerFaker(true).Generate();
                var partNumber = faker.Random.AlphaNumeric(
                    faker.Random.Int(RepairOrderItem.MinimumLength, RepairOrderItem.MaximumLength));
                var description = faker.Random.AlphaNumeric(
                    faker.Random.Int(RepairOrderItem.MinimumLength, RepairOrderItem.MaximumLength));
                var saleCode = new SaleCodeFaker(true).Generate();
                var productCode = new ProductCodeFaker(true).Generate();
                var partType = faker.PickRandom<PartType>();
                var laborType = faker.PickRandom<ItemLaborType>();

                var partOrLabor = faker.Random.Bool();

                var part = partOrLabor
                    ? RepairOrderItemPart.Create(
                        InstallablePart.MaximumMoneyAmount,
                        InstallablePart.MaximumMoneyAmount,
                        InstallablePart.MaximumMoneyAmount,
                        InstallablePart.MaximumMoneyAmount,
                        TechAmount.Create(
                            faker.PickRandom<ItemLaborType>(),
                            (double)Math.Round(faker.Random.Decimal(1, 99), 2),
                            faker.PickRandom<SkillLevel>())
                        .Value, false)
                    .Value
                    : null;

                var labor = !partOrLabor
                    ? RepairOrderItemLabor.Create(
                        LaborAmount.Create(
                            faker.PickRandom<ItemLaborType>(),
                            (double)Math.Round(faker.Random.Decimal(1, 99), 2))
                        .Value,
                        TechAmount.Create(
                            faker.PickRandom<ItemLaborType>(),
                            (double)Math.Round(faker.Random.Decimal(1, 99), 2),
                            faker.PickRandom<SkillLevel>())
                        .Value)
                    .Value
                    : null;

                var result = RepairOrderItem.Create(manufacturer, partNumber, description, saleCode, productCode, partType, part, labor);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
