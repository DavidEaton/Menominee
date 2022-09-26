using Menominee.Common;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackage : Entity
    {
        public long InventoryItem { get; private set; }
        public double BasePartsAmount { get; private set; }
        public double BaseLaborAmount { get; private set; }
        public string Script { get; private set; }
        public bool IsDiscountable { get; private set; }
        public virtual List<InventoryItemPackageItem> Items { get; private set; } = new List<InventoryItemPackageItem>();
        public virtual List<InventoryItemPackagePlaceholder> Placeholders { get; private set; } = new List<InventoryItemPackagePlaceholder>();
        
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
