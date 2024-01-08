using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemPackagePlaceholdersGrid
    {
        [Parameter]
        public List<InventoryItemPackagePlaceholderToWrite> Placeholders { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }
        private InventoryItemPackagePlaceholderToWrite Placeholder { get; set; }
        private IEnumerable<InventoryItemPackagePlaceholderToWrite> SelectedPlaceholders { get; set; } = Enumerable.Empty<InventoryItemPackagePlaceholderToWrite>();
        private InventoryItemPackagePlaceholderToWrite SelectedPlaceholder { get; set; }
        private TelerikGrid<InventoryItemPackagePlaceholderToWrite> PlaceholdersGrid { get; set; }
        private List<PlaceholderTypeListItem> placeholderTypeList { get; set; } = new List<PlaceholderTypeListItem>();

        protected override Task OnParametersSetAsync()
        {
            if (Placeholders.Count > 0)
            {
                SelectedPlaceholder = Placeholders.FirstOrDefault();
                SelectedPlaceholders = new List<InventoryItemPackagePlaceholderToWrite> { SelectedPlaceholder };
                Placeholder = SelectedPlaceholder;
            }

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            foreach (PackagePlaceholderItemType item in Enum.GetValues(typeof(PackagePlaceholderItemType)))
            {
                placeholderTypeList.Add(new PlaceholderTypeListItem
                {
                    Text = EnumExtensions.GetDisplayName(item),
                    Value = item
                });
            }
        }

        private void OnPlaceholderRowSelected(GridRowClickEventArgs args)
        {
            Placeholder = args.Item as InventoryItemPackagePlaceholderToWrite;
        }

        protected void OnPlaceholderSelect(IEnumerable<InventoryItemPackagePlaceholderToWrite> placeholders)
        {
            SelectedPlaceholder = placeholders.FirstOrDefault();
            SelectedPlaceholders = new List<InventoryItemPackagePlaceholderToWrite> { SelectedPlaceholder };
        }

        private async Task OnDeletePlaceholderAsync()
        {
            if (SelectedPlaceholder != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedPlaceholder.Description}?", "Remove Placeholder"))
            {
                Placeholders.Remove(SelectedPlaceholder);
                SelectedPlaceholder = Placeholders.FirstOrDefault();
                SelectedPlaceholders = new List<InventoryItemPackagePlaceholderToWrite> { SelectedPlaceholder };
                PlaceholdersGrid.Rebind();
            }
        }

        private void OnAddPlaceholder()
        {
            InventoryItemPackagePlaceholderToWrite itemToAdd = new()
            {
                Id = SelectedPlaceholder.Id,
                DisplayOrder = SelectedPlaceholder.DisplayOrder,
                ItemType = SelectedPlaceholder.ItemType,
                Details = SelectedPlaceholder.Details,
                Description = SelectedPlaceholder.Description
            };

            Placeholders.Add(itemToAdd);
            PlaceholdersGrid.Rebind();
            SelectedPlaceholder = itemToAdd;
            SelectedPlaceholders = new List<InventoryItemPackagePlaceholderToWrite> { SelectedPlaceholder };
        }
    }

    public class PlaceholderTypeListItem
    {
        public string Text { get; set; }
        public PackagePlaceholderItemType Value { get; set; }
    }
}
