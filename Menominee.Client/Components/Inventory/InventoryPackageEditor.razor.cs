using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPackageEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Package";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private bool parametersSet = false;
        private FormMode ItemFormMode = FormMode.Unknown;

        private long ItemId { get; set; } = 0;      // TODO: ItemId won't work for new items since their Ids will all be 0
        private long PlaceholderId { get; set; } = 0;

        private bool CanDeleteItem { get => ItemId > 0; }
        private bool CanDeletePlaceholder { get => PlaceholderId > 0; }

        private TelerikGrid<InventoryPackageItemToWrite> ItemsGrid { get; set; }
        private TelerikGrid<InventoryPackagePlaceholderToWrite> PlaceholdersGrid { get; set; }
        private IEnumerable<InventoryPackageItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<InventoryPackageItemToWrite>();
        private InventoryPackageItemToWrite SelectedItem { get; set; }
        private IEnumerable<InventoryPackagePlaceholderToWrite> SelectedPlaceholders { get; set; } = Enumerable.Empty<InventoryPackagePlaceholderToWrite>();
        private InventoryPackagePlaceholderToWrite SelectedPlaceholder { get; set; }
        private InventoryPackagePlaceholderToWrite PlaceholderToAdd { get; set; }
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }

        public List<IEditorTool> EditTools { get; set; } =
            new List<IEditorTool>()
            {
            new FontFamily(),
            new FontSize(),
            new EditorButtonGroup(new Bold(), new Italic(), new Underline()),
            new EditorButtonGroup(new AlignLeft(), new AlignCenter(), new AlignRight()),
            new UnorderedList()
            };

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

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;

            parametersSet = true;

            if (Item?.Package == null)
            {
                // TODO: How do we handle improper manufacturer / productcode setups?  What if these don't exist yet?
                //Item.ManufacturerId = PackageMfrId;
                var manufacturer = await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Package);
                Item.Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(manufacturer);

                var productCodeId = (await productCodeDataService.GetAllProductCodesAsync(manufacturer.Id)).ToList().FirstOrDefault().Id;
                if (productCodeId > 0)
                    Item.ProductCode = ProductCodeHelper.ConvertReadToWriteDto(await productCodeDataService.GetProductCodeAsync(productCodeId));
                else
                    Item.ProductCode = new();

                Item.Package = new();
                Item.ItemType = InventoryItemType.Package;

                Title = "Add Package";
            }

            if (Item.Package.Items.Count > 0)
            {
                SelectedItem = Item.Package.Items.FirstOrDefault();
                SelectedItems = new List<InventoryPackageItemToWrite> { SelectedItem };
                ItemId = SelectedItem.Id;
            }

            if (Item.Package.Placeholders.Count > 0)
            {
                SelectedPlaceholder = Item.Package.Placeholders.FirstOrDefault();
                SelectedPlaceholders = new List<InventoryPackagePlaceholderToWrite> { SelectedPlaceholder };
                PlaceholderId = SelectedPlaceholder.Id;
            }
        }

        private void OnAddItem()
        {
            ItemFormMode = FormMode.Add;
        }

        private void OnAddPlaceholder()
        {
            InventoryPackagePlaceholderToWrite itemToAdd = new();
            itemToAdd.ItemType = PackagePlaceholderItemType.Part;
            itemToAdd.Quantity = 1;
            Item.Package.Placeholders.Add(itemToAdd);
            PlaceholdersGrid.Rebind();
            SelectedPlaceholder = itemToAdd;
            SelectedPlaceholders = new List<InventoryPackagePlaceholderToWrite> { SelectedPlaceholder };
        }

        private async Task OnDeleteItemAsync()
        {
            if (SelectedItem != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedItem.ItemNumber}?", "Remove Item"))
            {
                Item.Package.Items.Remove(SelectedItem);
                SelectedItem = Item.Package.Items.FirstOrDefault();
                SelectedItems = new List<InventoryPackageItemToWrite> { SelectedItem };
                ItemsGrid.Rebind();
            }
        }

        private async Task OnDeletePlaceholderAsync()
        {
            if (SelectedPlaceholder != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedPlaceholder.Description}?", "Remove Placeholder"))
            {
                Item.Package.Placeholders.Remove(SelectedPlaceholder);
                SelectedPlaceholder = Item.Package.Placeholders.FirstOrDefault();
                SelectedPlaceholders = new List<InventoryPackagePlaceholderToWrite> { SelectedPlaceholder };
                PlaceholdersGrid.Rebind();
            }
        }

        protected void OnItemSelect(IEnumerable<InventoryPackageItemToWrite> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedItems = new List<InventoryPackageItemToWrite> { SelectedItem };
        }

        private void OnItemRowSelected(GridRowClickEventArgs args)
        {
            ItemId = (args.Item as InventoryPackageItemToWrite).Id;
        }

        protected void OnPlaceholderSelect(IEnumerable<InventoryPackagePlaceholderToWrite> placeholders)
        {
            SelectedPlaceholder = placeholders.FirstOrDefault();
            SelectedPlaceholders = new List<InventoryPackagePlaceholderToWrite> { SelectedPlaceholder };
        }

        private void OnPlaceholderRowSelected(GridRowClickEventArgs args)
        {
            PlaceholderId = (args.Item as InventoryPackagePlaceholderToWrite).Id;
        }

        protected async Task SubmitAddItemHandlerAsync()
        {
            InventoryPackageItemToWrite ItemToAdd = new();
            ItemToAdd.Item = new();
            ItemToAdd.InventoryItemId = SelectedInventoryItem.Id;
            ItemToAdd.Item.Id = SelectedInventoryItem.Id;
            ItemToAdd.Item.Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(await manufacturerDataService.GetManufacturerAsync(SelectedInventoryItem.ManufacturerId));
            ItemToAdd.Item.ProductCode = ProductCodeHelper.ConvertReadToWriteDto(await productCodeDataService.GetProductCodeAsync(SelectedInventoryItem.ProductCodeId));
            ItemToAdd.Item.ItemNumber = SelectedInventoryItem.ItemNumber;
            ItemToAdd.Item.Description = SelectedInventoryItem.Description;
            ItemToAdd.Quantity = 1;
            ItemToAdd.PartAmountIsAdditional = false;
            ItemToAdd.LaborAmountIsAdditional = false;
            ItemToAdd.ExciseFeeIsAdditional = false;
            Item.Package.Items.Add(ItemToAdd);
            EndAddItem();
            ItemsGrid.Rebind();
        }

        protected void EndAddItem()
        {
            ItemFormMode = FormMode.Unknown;
        }

        public class PlaceholderTypeListItem
        {
            public string Text { get; set; }
            public PackagePlaceholderItemType Value { get; set; }
        }

        private List<PlaceholderTypeListItem> placeholderTypeList { get; set; } = new List<PlaceholderTypeListItem>();
    }
}
