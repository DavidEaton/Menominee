using Bogus;
using Menominee.Domain.Entities;

namespace Menominee.TestingHelperLibrary.Fakers
{
    public static class SaleCodeMaker
    {
        private static readonly Dictionary<string, string> SaleCodes = new Dictionary<string, string>
        {
            {"A", "Alignments"},
            {"B", "Brakes"},
            {"C", "CCoil & Sprg"},
            {"D", "Exhaust FW"},
            {"E", "Exhaust"},
            {"F", "Chassis"},
            {"FM", "Factory Maintenance Progr"},
            {"G", "Suspension Fleet"},
            {"H", "Air Cond"},
            {"I", "Chassis Fleet"},
            {"J", "Mac Struts FW"},
            {"K", "Alignment FW"},
            {"L", "Lube Oil & Filter"},
            {"M", "Mac Struts"},
            {"N", "CC/Sprng FW"},
            {"O", "Cooling Systm"},
            {"P", "Other FW"},
            {"Q", "Brakes FW"},
            {"R", "Batteries"},
            {"S", "Suspension"},
            {"SS", "Shop Supplies"},
            {"T", "Start & Chrg"},
            {"U", "Other Retail"},
            {"V", "Transmission"},
            {"W", "Visibility"},
            {"X", "Tire Service"},
            {"Y", "Tires"},
            {"Z", "Other Repairs"},
            {"Z1", "Tire Levy"},
            {"Z2", "Towing"},
            {"ZZ", "Non Royalty Parts"}
        };

        public static List<SaleCode> GenerateSaleCodes(SaleCodeShopSupplies saleCodeShopSupplies = null)
        {
            var faker = new Faker<SaleCode>()
                .CustomInstantiator(faker =>
                {
                    var code = faker.PickRandom(SaleCodes.Keys.ToList());
                    var name = SaleCodes[code];
                    var laborRate = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                    var desiredMargin = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                    var percentage = (double)faker.Random.Decimal(0.01M, 1M);
                    var minimumJobAmount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                    var minimumCharge = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                    var maximumCharge = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                    var includeParts = faker.Random.Bool();
                    var includeLabor = faker.Random.Bool();
                    var shopSupplies = saleCodeShopSupplies is not null
                    ? saleCodeShopSupplies
                    : SaleCodeShopSupplies.Create(percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor).Value;

                    var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies, new List<string>());

                    return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
                });

            return SaleCodes.Keys.Select(_ => faker.Generate()).ToList();
        }
    }
}
