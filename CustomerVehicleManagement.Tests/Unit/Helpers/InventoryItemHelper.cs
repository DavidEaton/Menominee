using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
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

        public static Manufacturer CreateManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
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
            return new ProductCode()
            {
                Name = "A Product",
                Code = "P1"
            };
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

            var details = InventoryItemPackageDetails.Create(
                quantity,
                partAmountIsAdditional,
                laborAmountIsAdditional,
                exciseFeeIsAdditional)
                .Value;
            return details;
        }

        internal static InventoryItemPackagePlaceholder CreateInventoryItemPackagePlaceholder()
        {
            var details = CreateInventoryItemPackageDetails();

            return InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                "description",
                InventoryItemPackagePlaceholder.MinimumLength + 1,
                details).Value;
        }
    }
}
