using Menominee.Common;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackage : Entity
    {
        public long InventoryItemId { get; set; }
        public double BasePartsAmount { get; set; }
        public double BaseLaborAmount { get; set; }
        public string Script { get; set; }
        public bool IsDiscountable { get; set; }
        public virtual List<InventoryItemPackageItem> Items { get; set; } = new List<InventoryItemPackageItem>();
        public virtual List<InventoryItemPackagePlaceholder> Placeholders { get; set; } = new List<InventoryItemPackagePlaceholder>();
        
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

        // EF requires an empty constructor
        public InventoryItemPackage() { }

        #endregion 
    }
}
