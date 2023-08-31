using Bogus;
using Menominee.Domain.Entities;
using TestingHelperLibrary.Fakers;

namespace Menominee.TestingHelperLibrary.Fakers
{
    public class SaleCodeIshFaker : Faker<SaleCode>
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

        public SaleCodeIshFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => faker.Random.Long(1, 10000));

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id);

            CustomInstantiator(faker =>
            {
                var code = faker.PickRandom(SaleCodes.Keys.ToList());
                var name = SaleCodes[code];
                var laborRate = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var desiredMargin = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var shopSupplies = new SaleCodeShopSuppliesFaker(generateId).Generate();

                var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies, new List<string>());

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }

        public List<SaleCode> GenerateSaleCodes()
        {
            return SaleCodes.Keys.Select(_ => this.Generate()).ToList();
        }
    }

}
