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

        public PackagePlaceholderItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public InventoryItemPackageDetails Details { get; private set; }

        private InventoryItemPackagePlaceholder(PackagePlaceholderItemType itemType, string description, int displayOrder, InventoryItemPackageDetails details)
        {
            if (!Enum.IsDefined(typeof(PackagePlaceholderItemType), itemType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            ItemType = itemType;
            Description = description;
            DisplayOrder = displayOrder;
            Details = details;
        }

        public static Result<InventoryItemPackagePlaceholder> Create(PackagePlaceholderItemType itemType, string description, int displayOrder, InventoryItemPackageDetails details)
        {
            if (!Enum.IsDefined(typeof(PackagePlaceholderItemType), itemType))
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<InventoryItemPackagePlaceholder>($"{InvalidMessage} You entered {description.Length} character(s).");

            return Result.Success(new InventoryItemPackagePlaceholder(itemType, description, displayOrder, details));
        }

        public Result<PackagePlaceholderItemType> SetItemType(PackagePlaceholderItemType itemType)
        {
            if (!Enum.IsDefined(typeof(PackagePlaceholderItemType), itemType))
                return Result.Failure<PackagePlaceholderItemType>(RequiredMessage);

            return Result.Success(ItemType = itemType);
        }

        public Result<int> SetDisplayOrder(int displayOrder)
        {
            // TODO: parse/vaidate parameter: 
            // Invariants from Al: Min of 0 or 1, Max will be the number of records.
            return Result.Success(DisplayOrder = displayOrder);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<string>($"{InvalidMessage} You entered {description.Length} character(s).");

            return Result.Success(Description = description);
        }

        public Result<InventoryItemPackageDetails> SetDetails(InventoryItemPackageDetails details)
        {
            // deatils paremeter has already been validated
            // by the time we get here (type InventoryItemPackageDetails
            // Value Object)
            return Result.Success(Details = details);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
