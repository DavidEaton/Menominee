using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemPackageItemsGrid
    {
        [Parameter]
        public IReadOnlyList<InventoryItemPackageItemToWrite> Items { get; set; }

        public InventoryItemPackageItemToWrite Item { get; set; }

        private FormMode ItemFormMode = FormMode.Unknown;
        //private bool CanDeleteItem { get => Item.Id > 0; }
        //private bool CanDeletePlaceholder { get => Placeholder.Id > 0; }
        private InventoryItemPackageItemToWrite SelectedItem { get; set; }
        private IEnumerable<InventoryItemPackageItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<InventoryItemPackageItemToWrite>();

        private TelerikGrid<InventoryItemPackageItemToWrite> ItemsGrid { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        protected override Task OnParametersSetAsync()
        {
            if (Items.Count > 0)
            {
                SelectedItem = Items.FirstOrDefault();
                SelectedItems = new List<InventoryItemPackageItemToWrite> { SelectedItem };
                // TODO: remove next line after refactor
                //ItemId = SelectedItem.Id;
            }

            return Task.CompletedTask;
        }

        protected void OnItemSelect(IEnumerable<InventoryItemPackageItemToWrite> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedItems = new List<InventoryItemPackageItemToWrite> { SelectedItem };
        }

        private async Task OnDeleteItemAsync()
        {
            if (SelectedItem != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedItem.Item.ItemNumber}?", "Remove Item"))
            {
                //Items.Remove(SelectedItem);
                //SelectedItem = Item.Package.Items.FirstOrDefault();
                SelectedItems = new List<InventoryItemPackageItemToWrite> { SelectedItem };
                ItemsGrid.Rebind();
            }
        }

        private void OnItemRowSelected(GridRowClickEventArgs args)
        {
            Item = args.Item as InventoryItemPackageItemToWrite;
        }

        private void OnAddItem()
        {
            ItemFormMode = FormMode.Add;
        }
    }
}
