using Bogus;
using Menominee.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class SaleCodeFaker : Faker<SaleCode>
    {
        public SaleCodeFaker(bool generateId = false, long id = 0, bool generateFullList = false)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            if (generateFullList)
            {
                foreach (var saleCode in SaleCodes)
                {
                    CustomInstantiator(faker =>
                    {
                        var name = saleCode.Value;
                        var code = saleCode.Key;
                        var laborRate = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                        var desiredMargin = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                        var shopSupplies = new SaleCodeShopSuppliesFaker(generateId: generateId, id: id).Generate();

                        var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies, new List<string>());

                        return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
                    });
                }
            }

            if (!generateFullList)
            {
                CustomInstantiator(faker =>
                {
                    var name = faker.Random.String2(SaleCode.MinimumLength, SaleCode.NameMaximumLength);
                    var code = faker.Random.String2(SaleCode.MinimumLength, SaleCode.CodeMaximumLength);
                    var laborRate = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                    var desiredMargin = (double)Math.Round(faker.Random.Decimal((decimal)SaleCode.MinimumValue, (decimal)SaleCode.MaximumDesiredMarginValue), 2);
                    var shopSupplies = new SaleCodeShopSuppliesFaker(generateId: generateId, id: id).Generate();

                    var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies, new List<string>());

                    return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
                });
            }
        }
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
    }
}
