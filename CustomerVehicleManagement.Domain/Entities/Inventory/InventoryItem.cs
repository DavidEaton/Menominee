using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItem : Entity
    {
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidMessage = $"Item Number, Description must be between {MinimumLength} and {MaximumLength} character(s) in length.";
        public static readonly string RequiredMessage = $"Please include all required items.";

        public Manufacturer Manufacturer { get; private set; } //required
        public string ItemNumber { get; private set; } //required
        public string Description { get; private set; } //required
        public ProductCode ProductCode { get; private set; } //required
        public InventoryItemType ItemType { get; private set; } //required
        public InventoryItemPart Part { get; private set; }
        public InventoryItemLabor Labor { get; private set; }
        public InventoryItemTire Tire { get; private set; }
        public InventoryItemPackage Package { get; private set; }
        public InventoryItemInspection Inspection { get; private set; }
        public InventoryItemWarranty Warranty { get; private set; }

        private InventoryItem(Manufacturer manufacturer, string itemNumber, string description, ProductCode productCode, InventoryItemType itemType, InventoryItemPart part = null, InventoryItemLabor labor = null, InventoryItemTire tire = null, InventoryItemPackage package = null, InventoryItemInspection inspection = null, InventoryItemWarranty warranty = null)
        {
            if (manufacturer is null || productCode is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            itemNumber = (itemNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (itemNumber.Length < MinimumLength || itemNumber.Length > MaximumLength)
                throw new ArgumentOutOfRangeException($"{InvalidMessage} You entered {itemNumber.Length} character(s).");

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                throw new ArgumentOutOfRangeException($"{InvalidMessage} You entered {description.Length} character(s).");

            if (!Enum.IsDefined(typeof(InventoryItemType), itemType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Part)
                if (part is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Labor)
                if (labor is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Tire)
                if (tire is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Inspection)
                if (inspection is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Package)
                if (package is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            if (itemType == InventoryItemType.Warranty)
                if (warranty is null)
                    throw new ArgumentOutOfRangeException(RequiredMessage);

            Manufacturer = manufacturer;
            ItemNumber = itemNumber;
            Description = description;
            ProductCode = productCode;
            ItemType = itemType;

            bool validOptions = new[]
            {
                part is not null,
                labor is not null,
                tire is not null,
                inspection is not null,
                package is not null,
                warranty is not null
            }.Count(x => x == true) == 1;

            if (!validOptions)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            Part = part;
            Labor = labor;
            Tire = tire;
            Package = package;
            Inspection = inspection;
            Warranty = warranty;
        }

        public static Result<InventoryItem> Create(
            Manufacturer manufacturer,
            string itemNumber,
            string description,
            ProductCode productCode,
            InventoryItemType itemType,
            InventoryItemPart part = null,
            InventoryItemLabor labor = null,
            InventoryItemTire tire = null,
            InventoryItemPackage package = null,
            InventoryItemInspection inspection = null,
            InventoryItemWarranty warranty = null)
        {
            if (manufacturer is null || productCode is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            itemNumber = (itemNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (itemNumber.Length < MinimumLength || itemNumber.Length > MaximumLength)
                return Result.Failure<InventoryItem>($"{InvalidMessage} You entered {itemNumber.Length} character(s).");

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<InventoryItem>($"{InvalidMessage} You entered {description.Length} character(s).");

            if (!Enum.IsDefined(typeof(InventoryItemType), itemType))
                return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Part)
                if (part is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Labor)
                if (labor is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Tire)
                if (tire is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Inspection)
                if (inspection is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Package)
                if (package is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            if (itemType == InventoryItemType.Warranty)
                if (warranty is null)
                    return Result.Failure<InventoryItem>(RequiredMessage);

            // Enforce invariant: one and only one optional member
            bool validOptions = new[]
            {
                part is not null,
                labor is not null,
                tire is not null,
                inspection is not null,
                package is not null,
                warranty is not null
            }.Count(x => x == true) == 1;

            return !validOptions
                ? Result.Failure<InventoryItem>(RequiredMessage)
                : Result.Success(new InventoryItem(manufacturer, itemNumber, description, productCode, itemType, part, labor, tire, package, inspection, warranty));
        }

        public Result<Manufacturer> SetManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return Result.Failure<Manufacturer>(RequiredMessage);

            return Result.Success(Manufacturer = manufacturer);
        }

        public Result<ProductCode> SetProductCode(ProductCode productCode)
        {
            if (productCode is null)
                return Result.Failure<ProductCode>(RequiredMessage);

            return Result.Success(ProductCode = productCode);
        }

        public Result<string> SetItemNumber(string itemNumber)
        {
            itemNumber = (itemNumber ?? string.Empty).Trim();

            if (itemNumber.Length < MinimumLength || itemNumber.Length > MaximumLength)
                return Result.Failure<string>($"{InvalidMessage} You entered {itemNumber.Length} character(s).");

            return Result.Success(ItemNumber = itemNumber);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<string>($"{InvalidMessage} You entered {description.Length} character(s).");

            return Result.Success(Description = description);
        }

        // TODO: Clear all other InventoryItem{Type}s when calling SetItemType?
        private Result<InventoryItemType> SetItemType(InventoryItemType itemType)
        {
            if (!Enum.IsDefined(typeof(InventoryItemType), itemType))
                return Result.Failure<InventoryItemType>(RequiredMessage);

            return Result.Success(ItemType = itemType);
        }

        public Result<InventoryItemPart> SetPart(InventoryItemPart part)
        {
            if (part is null)
                return Result.Failure<InventoryItemPart>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Part);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemPart>(resltOrError.Error);

            return Result.Success(Part = part);
        }

        public Result<InventoryItemLabor> SetLabor(InventoryItemLabor labor)
        {
            if (labor is null)
                return Result.Failure<InventoryItemLabor>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Part);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemLabor>(resltOrError.Error);

            return Result.Success(Labor = labor);
        }

        public Result<InventoryItemTire> SetTire(InventoryItemTire tire)
        {
            if (tire is null)
                return Result.Failure<InventoryItemTire>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Tire);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemTire>(resltOrError.Error);

            return Result.Success(Tire = tire);
        }

        public Result<InventoryItemPackage> SetPackage(InventoryItemPackage package)
        {
            if (package is null)
                return Result.Failure<InventoryItemPackage>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Package);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemPackage>(resltOrError.Error);

            return Result.Success(Package = package);
        }

        public Result<InventoryItemInspection> SetInspection(InventoryItemInspection inspection)
        {
            if (inspection is null)
                return Result.Failure<InventoryItemInspection>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Inspection);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemInspection>(resltOrError.Error);

            return Result.Success(Inspection = inspection);
        }

        public Result<InventoryItemWarranty> SetWarranty(InventoryItemWarranty warranty)
        {
            if (warranty is null)
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            var resltOrError = SetItemType(InventoryItemType.Warranty);

            if (resltOrError.IsFailure)
                return Result.Failure<InventoryItemWarranty>(resltOrError.Error);

            return Result.Success(Warranty = warranty);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItem() { }

        #endregion
    }
}
