using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Common.Enums;
using TestingHelperLibrary.Fakers;

namespace TestingHelperLibrary
{
    public class InventoryItemTestHelper
    {
        public static InventoryItem CreateInventoryItem()
        {
            var manufacturer = CreateManufacturer();
            var productCode = CreateProductCode();
            var part = CreateInventoryItemPart();

            return InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Part, part: part).Value;
        }

        public static List<InventoryItem> CreateInventoryItems(int count)
        {
            var manufacturers = CreateManufacturers(count);
            var productCodes = CreateProductCodes(count);
            var parts = CreateInventoryItemParts(count);
            var inventoryItems = new List<InventoryItem>();

            for (int i = 0; i < count; i++)
            {
                inventoryItems.Add(
                    InventoryItem.Create(
                        manufacturers[i], $"001{i}", $"description number: {i}", productCodes[i], InventoryItemType.Part, part: parts[i])
                    .Value);
            }

            return inventoryItems;
        }

        public static InventoryItemPackage CreateInventoryItemPackage()
        {
            var basePartsAmount = InventoryItemPackage.MinimumAmount;
            var baseLaborAmount = InventoryItemPackage.MinimumAmount;
            var script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength - 100);
            var isDiscountable = true;
            List<InventoryItemPackageItem> items = new();
            List<InventoryItemPackagePlaceholder> placeholders = new();

            return InventoryItemPackage.Create(
                basePartsAmount,
                baseLaborAmount,
                script,
                isDiscountable,
                items,
                placeholders)
                .Value;
        }
        public static InventoryItemTire CreateInventoryItemTire()
        {
            var fractional = false;
            var width = InventoryItemTire.MaximumWidth;
            var aspectRatio = 65;
            var constructionType = TireConstructionType.R;
            var diameter = InventoryItemTire.MaximumDiameter;

            return InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional,
                type: "P", loadIndex: 89, speedRating: "H").Value;
        }

        public static ProductCode CreateProductCode()
        {
            Manufacturer manufacturer = CreateManufacturer();
            SaleCode saleCode = CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" }; //TODO: replace hack for compiler with real list

            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode).Value;
        }

        public static IReadOnlyList<ProductCode> CreateProductCodes(int count)
        {
            var manufacturers = CreateManufacturers(count);
            var saleCodes = CreateSaleCodes(count); //TODO: replace with Faker list
            List<string> manufacturerCodes = new() { "11" }; //TODO: replace hack for compiler with Faker list
            var productCodes = new List<ProductCode>();

            for (int i = 0; i < count; i++)
                productCodes.Add(
                    ProductCode.Create(
                        manufacturers[i], $"A{i}", $"A {i}{i}", manufacturerCodes, saleCodes[i])
                    .Value);

            return productCodes;
        }

        public static InventoryItemInspection CreateInventoryItemInspection()
        {
            return InventoryItemInspection.Create(
                LaborAmount.Create(ItemLaborType.Flat, 11.1).Value,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                InventoryItemInspectionType.CourtesyCheck).Value;
        }

        internal static IReadOnlyList<InventoryItemPart> CreateInventoryItemParts(int count)
        {
            var list = new List<InventoryItemPart>();

            for (int i = 0; i < count; i++)
            {
                list.Add(InventoryItemPart.Create(
                    list: 0.1 * i,
                    cost: 0.2 * i,
                    core: 0.3 * i,
                    retail: 0.4 * i,
                    techAmount: TechAmount.Create(ItemLaborType.Flat, 33.33, SkillLevel.A).Value,
                    fractional: false
                    ).Value);
            }

            return list;
        }

        public static InventoryItemPart CreateInventoryItemPart()
        {
            var list = InstallablePart.MinimumMoneyAmount;
            var cost = InstallablePart.MinimumMoneyAmount;
            var core = InstallablePart.MinimumMoneyAmount;
            var retail = InstallablePart.MinimumMoneyAmount;
            return InventoryItemPart.Create(
                list, cost, core, retail,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false).Value;
        }

        public static InventoryItemLabor CreateInventoryItemLabor()
        {
            return InventoryItemLabor.Create(
                LaborAmount.Create(ItemLaborType.Flat, 11.1).Value,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value).Value;
        }

        public static InventoryItemPackageItem CreateInventoryItemPackageItem()
        {
            var details = CreateInventoryItemPackageDetails();

            return InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                CreateInventoryItem(),
                details)
                .Value;
        }

        public static InventoryItemPackageDetails CreateInventoryItemPackageDetails()
        {
            var quantity = InventoryItemPackageDetails.MinimumValue + 1;
            var partAmountIsAdditional = true;
            var laborAmountIsAdditional = true;
            var exciseFeeIsAdditional = true;

            return InventoryItemPackageDetails.Create(
                quantity,
                partAmountIsAdditional,
                laborAmountIsAdditional,
                exciseFeeIsAdditional)
                .Value;
        }

        public static InventoryItemWarranty CreateInventoryItemWarranty()
        {
            return InventoryItemWarranty.Create(
                InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years, 3).Value
                    ).Value;
        }

        public static InventoryItemPackagePlaceholder CreateInventoryItemPackagePlaceholder()
        {
            var details = CreateInventoryItemPackageDetails();

            return InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                "description",
                InventoryItemPackagePlaceholder.DescriptionMinimumLength + 1,
                details).Value;
        }

        public static Manufacturer CreateManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        }

        public static List<Manufacturer> CreateManufacturers(int count)
        {
            var list = new List<Manufacturer>();

            for (int i = 0; i < count; i++)
            {
                list.Add(Manufacturer.Create($"Manufacturer {i}", "M{i}", "V{i}").Value);
            }

            return list;
        }

        public static SaleCode CreateSaleCode()
        {
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var laborRate = SaleCode.MinimumValue;
            var desiredMargin = SaleCode.MinimumValue;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true);

            return SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value;
        }

        public static List<SaleCode> CreateSaleCodes(int count)
        {
            var list = new List<SaleCode>();

            for (int i = 0; i < count; i++)
            {
                var name = Utilities.RandomCharacters(SaleCode.MinimumLength + count);
                var code = Utilities.RandomCharacters(SaleCode.MinimumLength + count);
                var laborRate = SaleCode.MinimumValue + count;
                var desiredMargin = SaleCode.MinimumValue + count;
                var shopSupplies = new SaleCodeShopSuppliesFaker(true);
                list.Add(SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value);
            }

            return list;
        }

        public static InventoryItemPartToWrite CreateInventoryItemPartToWrite()
        {
            return new()
            {
                List = InstallablePart.MaximumMoneyAmount,
                Cost = InstallablePart.MaximumMoneyAmount,
                Core = InstallablePart.MinimumMoneyAmount,
                Retail = InstallablePart.MinimumMoneyAmount,
                Fractional = false,
                TechAmount = new TechAmountToWrite
                {
                    PayType = ItemLaborType.Flat,
                    Amount = LaborAmount.MinimumAmount,
                    SkillLevel = SkillLevel.A
                }
            };
        }

        internal static List<ManufacturerToWrite> CreateManufacturersToWrite(int count)
        {
            var list = new List<ManufacturerToWrite>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new ManufacturerToWrite()
                {
                    Name = $"Manufacturer {i}",
                    Code = $"M{i}",
                    Prefix = $"V{i}"
                });
            }

            return list;
        }
    }
}
