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
        public static readonly int MinimumValue = 0;
        public static readonly int MaximumValue = 99999;
        public double BasePartsAmount { get; private set; }
        public double BaseLaborAmount { get; private set; }
        public string Script { get; private set; }
        public bool IsDiscountable { get; private set; } = false;
        public List<InventoryItemPackageItem> Items { get; private set; } = new List<InventoryItemPackageItem>();
        public List<InventoryItemPackagePlaceholder> Placeholders { get; private set; } = new List<InventoryItemPackagePlaceholder>();

        private InventoryItemPackage(double basePartsAmount, double baseLaborAmount, string script, bool isDiscountable, List<InventoryItemPackageItem> items, List<InventoryItemPackagePlaceholder> placeholders)
        {
            if (basePartsAmount < MinimumValue ||
                baseLaborAmount < MinimumValue ||
                basePartsAmount > MaximumValue ||
                baseLaborAmount > MaximumValue)
                throw new ArgumentOutOfRangeException(RequiredMessage);

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
            if (basePartsAmount < MinimumValue ||
                baseLaborAmount < MinimumValue ||
                basePartsAmount > MaximumValue ||
                baseLaborAmount > MaximumValue)
                return Result.Failure<InventoryItemPackage>(RequiredMessage);

            script = (script ?? string.Empty).Trim().Truncate(ScriptMaximumLength);

            if (!string.IsNullOrWhiteSpace(script) && script.Length > ScriptMaximumLength)
                return Result.Failure<InventoryItemPackage>(ScriptMaximumLengthMessage);

            return Result.Success(new InventoryItemPackage(basePartsAmount, baseLaborAmount, script, isDiscountable, items, placeholders));
        }


        public void AddItem(InventoryItemPackageItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(InventoryItemPackageItem item)
        {
            Items.Remove(item);
        }

        public void SetItems(IList<InventoryItemPackageItem> items)
        {
            if (items.Count > 0)
            {
                Items.Clear();
                foreach (var item in items)
                    AddItem(item);
            }
        }

        public void AddPlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            Placeholders.Add(placeholder);
        }

        public void RemovePlaceholder(InventoryItemPackagePlaceholder placeholder)
        {
            Placeholders.Remove(placeholder);
        }

        public void SetPlaceholders(IList<InventoryItemPackagePlaceholder> placeholders)
        {
            if (placeholders.Count > 0)
            {
                placeholders.Clear();
                foreach (var placeholder in placeholders)
                    AddPlaceholder(placeholder);
            }
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackage() { }

        #endregion 
    }
}
