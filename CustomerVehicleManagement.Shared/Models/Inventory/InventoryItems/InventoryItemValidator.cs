using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemToWrite>
    {
        private readonly Manufacturer validManufacturer = Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        private readonly ProductCode validProductCode = new()
        {
            Name = "A Product",
            Code = "P1"
        };

        public InventoryItemValidator()
        {
            RuleFor(itemDto => itemDto)
                .MustBeEntity(
                    itemDto => InventoryItem.Create(
                        validManufacturer,
                        itemDto.ItemNumber,
                        itemDto.Description,
                        validProductCode,
                        itemDto.ItemType,

                        itemDto.Part == null
                        ? null
                        : InventoryItemPart.Create(
                            itemDto.Part.List,
                            itemDto.Part.Cost,
                            itemDto.Part.Core,
                            itemDto.Part.Retail,
                            TechAmount.Create(
                                itemDto.Part.TechAmount.PayType,
                                itemDto.Part.TechAmount.Amount,
                                itemDto.Part.TechAmount.SkillLevel)
                            .Value,
                            itemDto.Part.Fractional,
                            itemDto.Part.LineCode,
                            itemDto.Part.SubLineCode)
                        .Value,

                        itemDto.Labor == null
                        ? null
                        : InventoryItemLabor.Create(
                            LaborAmount.Create(
                                itemDto.Labor.LaborAmount.PayType,
                                itemDto.Labor.LaborAmount.Amount)
                            .Value,
                            TechAmount.Create(
                                itemDto.Labor.TechAmount.PayType,
                                itemDto.Labor.TechAmount.Amount,
                                itemDto.Labor.TechAmount.SkillLevel)
                            .Value)
                        .Value,

                        itemDto.Tire == null
                        ? null
                        : InventoryItemTire.Create(
                            itemDto.Tire.Width,
                            itemDto.Tire.AspectRatio,
                            itemDto.Tire.ConstructionType,
                            itemDto.Tire.Diameter,
                            itemDto.Tire.List,
                            itemDto.Tire.Cost,
                            itemDto.Tire.Core,
                            itemDto.Tire.Retail,
                            TechAmount.Create(
                                itemDto.Tire.TechAmount.PayType,
                                itemDto.Tire.TechAmount.Amount,
                                itemDto.Tire.TechAmount.SkillLevel)
                            .Value,
                           itemDto.Tire.Fractional,
                           itemDto.Tire.LineCode,
                           itemDto.Tire.SubLineCode,
                           itemDto.Tire.Type,
                           itemDto.Tire.LoadIndex,
                           itemDto.Tire.SpeedRating)
                        .Value,

                        itemDto.Package == null
                        ? null
                        : InventoryItemPackage.Create(
                            itemDto.Package.BasePartsAmount,
                            itemDto.Package.BaseLaborAmount,
                            itemDto.Package.Script,
                            itemDto.Package.IsDiscountable,
                            itemDto.Package.Items == null
                            ? null
                            : new List<InventoryItemPackageItem>(),
                            itemDto.Package.Placeholders == null
                            ? null
                            : new List<InventoryItemPackagePlaceholder>())
                        .Value,

                        itemDto.Inspection == null
                        ? null
                        : InventoryItemInspection.Create(
                            LaborAmount.Create(
                                itemDto.Inspection.LaborAmount.PayType,
                                itemDto.Inspection.LaborAmount.Amount)
                            .Value,
                            TechAmount.Create(
                                itemDto.Inspection.TechAmount.PayType,
                                itemDto.Inspection.TechAmount.Amount,
                                itemDto.Inspection.TechAmount.SkillLevel)
                            .Value,
                            itemDto.Inspection.Type)
                        .Value,

                        itemDto.Warranty == null
                        ? null
                        : InventoryItemWarranty.Create(
                            InventoryItemWarrantyPeriod.Create(
                                itemDto.Warranty.PeriodType,
                                itemDto.Warranty.Duration)
                            .Value)
                        .Value));
        }
    }
}
