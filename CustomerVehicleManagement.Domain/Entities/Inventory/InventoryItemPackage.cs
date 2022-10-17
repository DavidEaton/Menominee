using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using System;
using System.Collections.Generic;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
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
        public List<InventoryItemPackageItem> Items { get; private set; } = new List<InventoryItemPackageItem>();
        public List<InventoryItemPackagePlaceholder> Placeholders { get; private set; } = new List<InventoryItemPackagePlaceholder>();

        private InventoryItemPackage(double basePartsAmount, double baseLaborAmount, string script, bool isDiscountable, List<InventoryItemPackageItem> items, List<InventoryItemPackagePlaceholder> placeholders)
        {
            if (basePartsAmount < MinimumAmount ||
                baseLaborAmount < MinimumAmount ||
                basePartsAmount > MaximumAmount ||
                baseLaborAmount > MaximumAmount)
                throw new ArgumentOutOfRangeException(InvalidAmountMessage);

            script = (script ?? string.Empty).Trim().Truncate(ScriptMaximumLength);

            if (!string.IsNullOrWhiteSpace(script) && script.Length > ScriptMaximumLength)
                throw new ArgumentOutOfRangeException(ScriptMaximumLengthMessage);

            BasePartsAmount = basePartsAmount;
            BaseLaborAmount = baseLaborAmount;
            Script = script;
            IsDiscountable = isDiscountable;
            Items = items ?? new List<InventoryItemPackageItem>();
            Placeholders = placeholders ?? new List<InventoryItemPackagePlaceholder>();
        }

        public static Result<InventoryItemPackage> Create(double basePartsAmount, double baseLaborAmount, string script, bool isDiscountable, List<InventoryItemPackageItem> items, List<InventoryItemPackagePlaceholder> placeholders)
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

        public Result AddItem(InventoryItemPackageItem item)
        {
            if (item is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            try
            {
                Items.Add(item);
            }
            catch (Exception ex)
            {
                return Result.Failure<InventoryItemPackageItem>($"Unable to add item: {item}");
                // Log exception details
            }

            return Result.Success();
        }

        public Result RemoveItem(InventoryItemPackageItem item)
        {
            if (item is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            try
            {
                Items.Remove(item);
            }
            catch (Exception ex)
            {
                return Result.Failure<InventoryItemPackageItem>($"Unable to remove item: {item}");
                // Log exception details
            }

            return Result.Success();
        }

        public Result AddPlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            if (placeholder is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);

            try
            {
                Placeholders.Add(placeholder);
            }
            catch (Exception ex)
            {
                return Result.Failure<InventoryItemPackagePlaceholder>($"Unable to add placeholder: {placeholder}");
                // Log exception details
            }

            return Result.Success();
        }

        public Result RemovePlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            if (placeholder is null)
                return Result.Failure<InventoryItemPackagePlaceholder>(RequiredMessage);
            try
            {
                Placeholders.Remove(placeholder);
            }
            catch (Exception ex)
            {
                return Result.Failure<InventoryItemPackagePlaceholder>($"Unable to remove placeholder: {placeholder}");
                // Log exception details
            }

            return Result.Success();
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackage() { }

        #endregion 
    }
}
