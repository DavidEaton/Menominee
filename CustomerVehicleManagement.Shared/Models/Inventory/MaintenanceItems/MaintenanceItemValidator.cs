using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using FluentValidation;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemValidator : AbstractValidator<MaintenanceItemToWrite>
    {
        // Create valid objects to use for parent (MaintenanceItemToWrite) read DTO
        // members (ManufacturerToRead, ProductCodeToRead, )
        private readonly Manufacturer validManufacturer = CreateValidManufacturer();
        private readonly ProductCode validProductCode = CreateValidProductCode();
        private readonly InventoryItem validInventoryItem = CreateValidInventryItem();

        // INSTEAD OF ALL THAT OBJECT CREATION BELOW, VALIDATE MEMBERS FIRST
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public MaintenanceItemValidator()
        {
            RuleFor(maintenanceItem => maintenanceItem)
                .MustBeEntity(
                    maintenanceItem => MaintenanceItem.Create(
                        maintenanceItem.DisplayOrder,
                        InventoryItem.Create(
                            validManufacturer,
                            maintenanceItem.Item.ItemNumber,
                            maintenanceItem.Item.Description,
                            validProductCode,
                            maintenanceItem.Item.ItemType,
                        maintenanceItem.Item.Part == null
                        ? null
                        : InventoryItemPart.Create(
                            maintenanceItem.Item.Part.List,
                            maintenanceItem.Item.Part.Cost,
                            maintenanceItem.Item.Part.Core,
                            maintenanceItem.Item.Part.Retail,
                            TechAmount.Create(
                                maintenanceItem.Item.Part.TechAmount.PayType,
                                maintenanceItem.Item.Part.TechAmount.Amount,
                                maintenanceItem.Item.Part.TechAmount.SkillLevel)
                            .Value,
                            maintenanceItem.Item.Part.Fractional,
                            maintenanceItem.Item.Part.LineCode,
                            maintenanceItem.Item.Part.SubLineCode)
                        .Value,

                        maintenanceItem.Item.Labor == null
                        ? null
                        : InventoryItemLabor.Create(
                            LaborAmount.Create(
                                maintenanceItem.Item.Labor.LaborAmount.PayType,
                                maintenanceItem.Item.Labor.LaborAmount.Amount)
                            .Value,
                            TechAmount.Create(
                                maintenanceItem.Item.Labor.TechAmount.PayType,
                                maintenanceItem.Item.Labor.TechAmount.Amount,
                                maintenanceItem.Item.Labor.TechAmount.SkillLevel)
                            .Value)
                        .Value,

                        maintenanceItem.Item.Tire == null
                        ? null
                        : InventoryItemTire.Create(
                            maintenanceItem.Item.Tire.Width,
                            maintenanceItem.Item.Tire.AspectRatio,
                            maintenanceItem.Item.Tire.ConstructionType,
                            maintenanceItem.Item.Tire.Diameter,
                            maintenanceItem.Item.Tire.List,
                            maintenanceItem.Item.Tire.Cost,
                            maintenanceItem.Item.Tire.Core,
                            maintenanceItem.Item.Tire.Retail,
                            TechAmount.Create(
                                maintenanceItem.Item.Tire.TechAmount.PayType,
                                maintenanceItem.Item.Tire.TechAmount.Amount,
                                maintenanceItem.Item.Tire.TechAmount.SkillLevel)
                            .Value,
                           maintenanceItem.Item.Tire.Fractional,
                           maintenanceItem.Item.Tire.LineCode,
                           maintenanceItem.Item.Tire.SubLineCode,
                           maintenanceItem.Item.Tire.Type,
                           maintenanceItem.Item.Tire.LoadIndex,
                           maintenanceItem.Item.Tire.SpeedRating)
                        .Value,

                        maintenanceItem.Item.Package == null
                        ? null
                        : InventoryItemPackage.Create(
                            maintenanceItem.Item.Package.BasePartsAmount,
                            maintenanceItem.Item.Package.BaseLaborAmount,
                            maintenanceItem.Item.Package.Script,
                            maintenanceItem.Item.Package.IsDiscountable,
                            maintenanceItem.Item.Package.Items == null
                            ? null
                            : CreatemPackageItems(maintenanceItem.Item.Package.Items),
                            maintenanceItem.Item.Package.Placeholders == null
                            ? null
                            : CreatePackagePlaceholders(maintenanceItem.Item.Package.Placeholders))
                        .Value,

                        maintenanceItem.Item.Inspection == null
                        ? null
                        : InventoryItemInspection.Create(
                            LaborAmount.Create(
                                maintenanceItem.Item.Inspection.LaborAmount.PayType,
                                maintenanceItem.Item.Inspection.LaborAmount.Amount)
                            .Value,
                            TechAmount.Create(
                                maintenanceItem.Item.Inspection.TechAmount.PayType,
                                maintenanceItem.Item.Inspection.TechAmount.Amount,
                                maintenanceItem.Item.Inspection.TechAmount.SkillLevel)
                            .Value,
                            maintenanceItem.Item.Inspection.Type)
                        .Value,

                        maintenanceItem.Item.Warranty == null
                        ? null
                        : InventoryItemWarranty.Create(
                            InventoryItemWarrantyPeriod.Create(
                                maintenanceItem.Item.Warranty.PeriodType,
                                maintenanceItem.Item.Warranty.Duration)
                            .Value)
                        .Value)
                    .Value));
        }

        private static InventoryItem CreateValidInventryItem()
        {
            double list = InstallablePart.MinimumMoneyAmount;
            double cost = InstallablePart.MinimumMoneyAmount;
            double core = InstallablePart.MinimumMoneyAmount;
            double retail = InstallablePart.MinimumMoneyAmount;
            var part = InventoryItemPart.Create(
                list, cost, core, retail,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false).Value;

            return InventoryItem.Create(
            CreateValidManufacturer(),
            "001",
            "a description",
            CreateValidProductCode(),
            InventoryItemType.Part,
            part: part).Value;
        }

        private static Manufacturer CreateValidManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        }

        private static ProductCode CreateValidProductCode()
        {
            return new ProductCode() { Name = "A Product", Code = "P1" };
        }

        private List<InventoryItemPackagePlaceholder> CreatePackagePlaceholders(List<InventoryItemPackagePlaceholderToWrite> placeholders)
        {
            var result = new List<InventoryItemPackagePlaceholder>();

            foreach (var placeholder in placeholders)
            {
                result.Add(
                    InventoryItemPackagePlaceholder.Create(
                        (PackagePlaceholderItemType)Enum.Parse(typeof(PackagePlaceholderItemType), placeholder.ItemType),
                        placeholder.Description,
                        placeholder.DisplayOrder,
                        CreateInventoryItemPackageDetails())
                    .Value);
            }

            return result;
        }

        private List<InventoryItemPackageItem> CreatemPackageItems(List<InventoryItemPackageItemToWrite> items)
        {
            var result = new List<InventoryItemPackageItem>();

            foreach (var item in items)
            {
                result.Add(
                    InventoryItemPackageItem.Create(
                        item.DisplayOrder,
                        validInventoryItem,
                        CreateInventoryItemPackageDetails())
                    .Value);
            }

            return result;
        }

        private InventoryItemPackageDetails CreateInventoryItemPackageDetails()
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
    }
}
