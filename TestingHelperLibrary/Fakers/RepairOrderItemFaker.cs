using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Enums;
using Menominee.Tests.Helpers.Fakers;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderItemFaker : Faker<RepairOrderItem>
    {
        public RepairOrderItemFaker(
            bool generateId = false,
            long id = 0,
            SaleCode saleCodeFromCaller = null,
            Manufacturer manufacturerFromCaller = null,
            ProductCode productCodeFromCaller = null)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var partPrefixes = new[] { "PT", "RD", "EN", "TR", "BR" };
                var description = CreateDescription(faker);
                var partType = faker.PickRandom<PartType>();
                var laborType = faker.PickRandom<ItemLaborType>();
                var lineCode = faker.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + faker.Random.String2(2, "0123456789");
                var subLineCode = faker.Random.AlphaNumeric(faker.Random.Int(2, 5));
                var partOrLabor = faker.Random.Bool();

                var manufacturer = manufacturerFromCaller is not null
                    ? manufacturerFromCaller
                    : new ManufacturerFaker(generateId).Generate();

                var partNumber = faker.PickRandom(partPrefixes)
                    + faker.Random.Number(1000, 9999)
                    + faker.Random.AlphaNumeric(2);
                partNumber = partNumber.Substring(0, Math.Min(partNumber.Length, RepairOrderItem.MaximumLength));

                var saleCode = saleCodeFromCaller is not null
                    ? saleCodeFromCaller
                    : new SaleCodeFaker(generateId).Generate();

                var productCode = productCodeFromCaller is not null
                    ? productCodeFromCaller
                    : new ProductCodeFaker(generateId).Generate();

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
                        .Value,
                        fractional: false,
                        lineCode: lineCode,
                        subLineCode: subLineCode)
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

        private static string CreateDescription(Faker faker)
        {
            var lengthLimit = faker.Random.Int(RepairOrderItem.MinimumLength, RepairOrderItem.MaximumLength);
            var words = new List<string>();

            while (string.Join(" ", words).Length < lengthLimit)
                words.Add(faker.Lorem.Word());

            return string
                .Join(" ", words)
                .Substring(0, lengthLimit)
                .Trim();
        }
    }
}
