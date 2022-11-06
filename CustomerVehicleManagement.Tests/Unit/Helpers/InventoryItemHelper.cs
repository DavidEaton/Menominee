using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.Unit.Helpers
{
    public class InventoryItemHelper
    {
        public static InventoryItem CreateInventoryItem()
        {
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();

            return InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Part, part: part).Value;
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
            Manufacturer manufacturer = CreateManufacturer();
            SaleCode saleCode = CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" }; //TODO: replace hack for compiler with real list

            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode).Value;
        }

        public static InventoryItemInspection CreateInventoryItemInspection()
        {
            return InventoryItemInspection.Create(
                LaborAmount.Create(ItemLaborType.Flat, 11.1).Value,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                InventoryItemInspectionType.CourtesyCheck).Value;
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

        internal static InventoryItemPackagePlaceholder CreateInventoryItemPackagePlaceholder()
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
    }
}
