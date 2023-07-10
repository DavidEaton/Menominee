using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class InventoryItemPackagePlaceholder : Entity
    {
        public static readonly int DescriptionMinimumLength = 1;
        public static readonly int DescriptionMaximumLength = 255;
        public static readonly string DescriptionInvalidMessage = $"Description must be between {DescriptionMinimumLength} and {DescriptionMaximumLength} character(s) in length.";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string DisplayOrderInvalidMessage = $"Display Order must be > 0.";

        public PackagePlaceholderItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public InventoryItemPackageDetails Details { get; private set; }

        private InventoryItemPackagePlaceholder(PackagePlaceholderItemType itemType, string description, int displayOrder, InventoryItemPackageDetails details)
        {
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

            if (description.Length < DescriptionMinimumLength || description.Length > DescriptionMaximumLength)
                return Result.Failure<InventoryItemPackagePlaceholder>($"{DescriptionInvalidMessage} You entered {description.Length} character(s).");

            if (displayOrder < 1)
                return Result.Failure<InventoryItemPackagePlaceholder>(DisplayOrderInvalidMessage);

            if (details is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

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
            if (displayOrder < 1)
                return Result.Failure<int>(DisplayOrderInvalidMessage);

            return Result.Success(DisplayOrder = displayOrder);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length < DescriptionMinimumLength || description.Length > DescriptionMaximumLength)
                return Result.Failure<string>($"{DescriptionInvalidMessage} You entered {description.Length} character(s).");

            return Result.Success(Description = description);
        }

        public Result<InventoryItemPackageDetails> SetDetails(InventoryItemPackageDetails details)
        {
            // deatils paremeter has already been validated
            // by the time we get here (type InventoryItemPackageDetails
            // Value Object)
            if (details is null)
                return Result.Failure<InventoryItemPackageDetails>(RequiredMessage);

            return Result.Success(Details = details);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
