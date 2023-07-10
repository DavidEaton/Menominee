using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
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
        private InventoryItemPackagePlaceholderToWrite SelectedPlaceholder;
        private long ItemId { get; set; } = 0;      // TODO: ItemId won't work for new items since their Ids will all be 0
        private InventoryItemPackagePlaceholderToWrite Placeholder { get; set; }

        private TelerikGrid<InventoryItemPackagePlaceholderToWrite> PlaceholdersGrid { get; set; }
        private InventoryItemPackagePlaceholderToWrite PlaceholderToAdd { get; set; }
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }
        public List<InventoryItemPackagePlaceholderToWrite> SelectedPlaceholders { get; set; }
        public List<IEditorTool> EditTools { get; set; } =
            new List<IEditorTool>()
            {
            new FontFamily(),
            new FontSize(),
            new EditorButtonGroup(new Bold(), new Italic(), new Underline()),
            new EditorButtonGroup(new AlignLeft(), new AlignCenter(), new AlignRight()),
            new UnorderedList()
            };

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;

            parametersSet = true;

            if (Item?.Package == null)
            {
                // TODO: How do we handle improper manufacturer / productcode setups?  What if these don't exist yet?
                //Item.ManufacturerId = PackageMfrId;
                Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Package);
                Item.ProductCode = await productCodeDataService.GetProductCodeAsync(
                    (await productCodeDataService.GetAllProductCodesAsync(
                        Item.Manufacturer.Id))
                        .ToList()
                        .FirstOrDefault().Id);
                Item.Package = new();
                Item.ItemType = InventoryItemType.Package;

                Title = "Add Package";
            }

        }

        private void OnAddPlaceholder()
        {
            InventoryItemPackagePlaceholderToWrite itemToAdd = new();
            itemToAdd.ItemType = PackagePlaceholderItemType.Part;
            itemToAdd.Details = new InventoryItemPackageDetailsToWrite()
            {
                ExciseFeeIsAdditional = false,
                LaborAmountIsAdditional = false,
                PartAmountIsAdditional = false,
                Quantity = 1
            };
            Item.Package.Placeholders.Add(itemToAdd);
            PlaceholdersGrid.Rebind();
            SelectedPlaceholder = itemToAdd;
            SelectedPlaceholders = new List<InventoryItemPackagePlaceholderToWrite> { SelectedPlaceholder };
        }






        protected async Task SubmitAddItemHandlerAsync()
        {
            InventoryItemPackageItemToWrite ItemToAdd = new()
            {
                Id = ItemId,
                DisplayOrder = 1,
                Item = new()
                {
                    Id = SelectedInventoryItem.Id,
                    Manufacturer = SelectedInventoryItem.Manufacturer,
                    ItemNumber = SelectedInventoryItem.ItemNumber,
                    Description = SelectedInventoryItem.Description,
                    ProductCode = SelectedInventoryItem.ProductCode,
                    ItemType = SelectedInventoryItem.ItemType
                },
                Quantity = 1,
                PartAmountIsAdditional = false,
                LaborAmountIsAdditional = false,
                ExciseFeeIsAdditional = false,
            };

            Item.Package.Items.Add(ItemToAdd);
            EndAddItem();
            //ItemsGrid.Rebind(); // TODO: signal child grid component to rebind (if necessary)
        }

        protected void EndAddItem()
        {
            ItemFormMode = FormMode.Unknown;
        }
    }
}
