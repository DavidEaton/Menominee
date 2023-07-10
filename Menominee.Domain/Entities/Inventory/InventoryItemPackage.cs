using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class InventoryItemPackage : Entity
    {
        public static readonly int ScriptMaximumLength = 10000;
        public static readonly string ScriptMaximumLengthMessage = $"Script cannot be over {ScriptMaximumLength} characters in length.";
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly int MinimumAmount = 0;
        public static readonly int MaximumAmount = 99999;
        public static readonly string InvalidAmountMessage = $"Amount must be within {MinimumAmount} and {MaximumAmount}.";

        public double BasePartsAmount { get; private set; }
        public double BaseLaborAmount { get; private set; }
        public string Script { get; private set; }
        public bool IsDiscountable { get; private set; } = false;

        private readonly List<InventoryItemPackageItem> items = new();
        public List<InventoryItemPackageItem> Items => items.ToList();

        private readonly List<InventoryItemPackagePlaceholder> placeholders = new();
        public List<InventoryItemPackagePlaceholder> Placeholders => placeholders.ToList();

        private InventoryItemPackage(double basePartsAmount, double baseLaborAmount, string script, bool isDiscountable, List<InventoryItemPackageItem> items = null, List<InventoryItemPackagePlaceholder> placeholders = null)
        {
            BasePartsAmount = basePartsAmount;
            BaseLaborAmount = baseLaborAmount;
            Script = script;
            IsDiscountable = isDiscountable;
            this.items = items ?? new List<InventoryItemPackageItem>();
            this.placeholders = placeholders ?? new List<InventoryItemPackagePlaceholder>();
        }

        public static Result<InventoryItemPackage> Create(double basePartsAmount, double baseLaborAmount, string script, bool isDiscountable, List<InventoryItemPackageItem> items = null, List<InventoryItemPackagePlaceholder> placeholders = null)
        {
            if (basePartsAmount < MinimumAmount ||
                baseLaborAmount < MinimumAmount ||
                basePartsAmount > MaximumAmount ||
                baseLaborAmount > MaximumAmount)
                return Result.Failure<InventoryItemPackage>(InvalidAmountMessage);

            script = (script ?? string.Empty).Trim().Truncate(ScriptMaximumLength);

            if (!string.IsNullOrWhiteSpace(script) && script.Length > ScriptMaximumLength)
                return Result.Failure<InventoryItemPackage>(ScriptMaximumLengthMessage);

            return Result.Success(new InventoryItemPackage(basePartsAmount, baseLaborAmount, script, isDiscountable, items, placeholders));
        }

        public Result<double> SetBasePartsAmount(double basePartsAmount)
        {
            if (basePartsAmount < MinimumAmount || basePartsAmount > MaximumAmount)
                return Result.Failure<double>(InvalidAmountMessage);

            return Result.Success(BasePartsAmount = basePartsAmount);
        }

        public Result<double> SetBaseLaborAmount(double baseLaborAmount)
        {
            if (baseLaborAmount < MinimumAmount || baseLaborAmount > MaximumAmount)
                return Result.Failure<double>(InvalidAmountMessage);

            return Result.Success(BaseLaborAmount = baseLaborAmount);
        }

        public Result<string> SetScript(string script)
        {
            script = (script ?? string.Empty).Trim().Truncate(ScriptMaximumLength);

            if (!string.IsNullOrWhiteSpace(script) && script.Length > ScriptMaximumLength)
                return Result.Failure<string>(ScriptMaximumLengthMessage);

            return Result.Success(Script = script);

        }

        public Result<bool> SetIsDiscountable(bool isDiscountable)
        {
            return Result.Success(IsDiscountable = isDiscountable);
        }

        public Result<InventoryItemPackageItem> AddItem(InventoryItemPackageItem item)
        {
            if (item is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            items.Add(item);

            return Result.Success(item);
        }

        public Result<InventoryItemPackageItem> RemoveItem(InventoryItemPackageItem item)
        {
            if (item is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            items.Remove(item);

            return Result.Success(item);

        }

        public Result<InventoryItemPackagePlaceholder> AddPlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            if (placeholder is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            placeholders.Add(placeholder);

            return Result.Success(placeholder);
        }

        public Result<InventoryItemPackagePlaceholder> RemovePlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            if (placeholder is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            placeholders.Remove(placeholder);

            return Result.Success(placeholder);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackage()
        {
            items = new List<InventoryItemPackageItem>();
            placeholders = new List<InventoryItemPackagePlaceholder>();
        }

        #endregion 
    }
}
