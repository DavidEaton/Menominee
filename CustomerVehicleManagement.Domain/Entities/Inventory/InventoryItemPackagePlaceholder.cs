using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackagePlaceholder : Entity
    {
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidMessage = $"Description must be between {MinimumLength} and {MaximumLength} character(s) in length.";
        public static readonly string RequiredMessage = $"Please include all required items.";

        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public PackagePlaceholderItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public InventoryItemPackageDetails InventoryItemPackageDetails { get; private set; }

        private InventoryItemPackagePlaceholder(InventoryItemPackage package, PackagePlaceholderItemType type, string description, int displayOrder, InventoryItemPackageDetails details)
        {
            if (package is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemType), type))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            InventoryItemPackage = package;
            ItemType = type;
            Description = description;
            DisplayOrder = displayOrder;
            InventoryItemPackageDetails = details;
        }

        public static Result<InventoryItemPackagePlaceholder> Create(InventoryItemPackage package, PackagePlaceholderItemType type, string description, int displayOrder, InventoryItemPackageDetails details)
        {
            if (package is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemType), type))
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<InventoryItemPackagePlaceholder>($"{InvalidMessage} You entered {description.Length} character(s).");

            return Result.Success(new InventoryItemPackagePlaceholder(package, type, description, displayOrder, details));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
