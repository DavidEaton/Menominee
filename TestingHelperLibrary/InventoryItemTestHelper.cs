using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Common.Enums;

namespace TestingHelperLibrary
{
    public class InventoryItemTestHelper
    {
        public static InventoryItem CreateInventoryItem()
        {
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();

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
            double basePartsAmount = InventoryItemPackage.MinimumAmount;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount;
            string script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength - 100);
            bool isDiscountable = true;
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
            int width = InventoryItemTire.MaximumWidth;
            int aspectRatio = 65;
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;

            return InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional,
                type: "P", loadIndex: 89, speedRating: "H").Value;
        }

        public static ProductCode CreateProductCode()
        {
            var manufacturer = CreateManufacturer();
            var saleCode = CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" }; //TODO: replace hack for compiler with real list

            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode).Value;
        }

        public static IReadOnlyList<ProductCode> CreateProductCodes(int count)
        {
            var manufacturers = CreateManufacturers(count);
            var saleCodes = CreateSaleCodes(count);
            List<string> manufacturerCodes = new() { "11" }; //TODO: replace hack for compiler with real list
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
            double list = InstallablePart.MinimumMoneyAmount;
            double cost = InstallablePart.MinimumMoneyAmount;
            double core = InstallablePart.MinimumMoneyAmount;
            double retail = InstallablePart.MinimumMoneyAmount;
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
            bool partAmountIsAdditional = true;
            bool laborAmountIsAdditional = true;
            bool exciseFeeIsAdditional = true;

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
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            return SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value;
        }

        public static List<SaleCode> CreateSaleCodes(int count)
        {
            var list = new List<SaleCode>();

            for (int i = 0; i < count; i++)
            {
                string name = Utilities.RandomCharacters(SaleCode.MinimumLength + count);
                string code = Utilities.RandomCharacters(SaleCode.MinimumLength + count);
                double laborRate = SaleCode.MinimumValue + count;
                double desiredMargin = SaleCode.MinimumValue + count;
                SaleCodeShopSupplies shopSupplies = new();
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
