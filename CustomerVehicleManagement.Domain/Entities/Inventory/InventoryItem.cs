using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItem : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double InvalidValue = 0;
        public static readonly string InvalidValueMessage = $"Value cannot be zero.";

        public Manufacturer Manufacturer { get; private set; }
        public string ItemNumber { get; private set; }
        public string Description { get; private set; }
        public ProductCode ProductCode { get; private set; }
        public InventoryItemType ItemType { get; private set; }

        public InventoryItemPart Part { get; private set; }
        public InventoryItemLabor Labor { get; private set; }
        public InventoryItemTire Tire { get; private set; }
        public InventoryItemPackage Package { get; private set; }
        public InventoryItemInspection Inspection { get; private set; }
        public InventoryItemWarranty Warranty { get; private set; }

        private InventoryItem(Manufacturer manufacturer, string itemNumber, string description, ProductCode productCode, InventoryItemType itemType, InventoryItemPart part, InventoryItemLabor labor, InventoryItemTire tire, InventoryItemPackage package, InventoryItemInspection inspection, InventoryItemWarranty warranty)
        {
            if (manufacturer is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);


            Manufacturer = manufacturer;
            ItemNumber = itemNumber;
            Description = description;
            ProductCode = productCode;
            ItemType = itemType;
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
            InventoryItemPart part,
            InventoryItemLabor labor,
            InventoryItemTire tire,
            InventoryItemPackage package,
            InventoryItemInspection inspection,
            InventoryItemWarranty warranty)
        {
            if (manufacturer is null)
                return Result.Failure<InventoryItem>(RequiredMessage);


            return Result.Success(new InventoryItem(manufacturer, itemNumber, description, productCode, itemType, part, labor, tire, package, inspection, warranty));
        }

        public InventoryItem(InventoryItemPart part)
        {
            if (part is null)
                throw new ArgumentOutOfRangeException(nameof(part), "part == null");

            Part = part;
            ItemType = InventoryItemType.Part;
        }

        public InventoryItem(InventoryItemLabor labor)
        {
            if (labor is null)
                throw new ArgumentOutOfRangeException(nameof(labor), "labor == null");

            Labor = labor;
            ItemType = InventoryItemType.Labor;
        }

        public InventoryItem(InventoryItemTire tire)
        {
            if (tire is null)
                throw new ArgumentOutOfRangeException(nameof(tire), "tire == null");

            Tire = tire;
            ItemType = InventoryItemType.Tire;
        }

        public InventoryItem(InventoryItemPackage package)
        {
            if (package is null)
                throw new ArgumentOutOfRangeException(nameof(package), "package == null");

            Package = package;
            ItemType = InventoryItemType.Package;
        }

        public InventoryItem(InventoryItemInspection inspection)
        {
            if (inspection is null)
                throw new ArgumentOutOfRangeException(nameof(inspection), "inspection == null");

            Inspection = inspection;
            ItemType = InventoryItemType.Inspection;
        }

        public InventoryItem(InventoryItemWarranty warranty)
        {
            if (warranty is null)
                throw new ArgumentOutOfRangeException(nameof(warranty), "warranty == null");

            Warranty = warranty;
            ItemType = InventoryItemType.Warranty;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItem() { }

        #endregion
    }
}
