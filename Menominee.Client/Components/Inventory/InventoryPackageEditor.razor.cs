using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Shared.Models.ProductCodes;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPackageEditor : InventoryEditorBase
    {
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; } = null!;

        private FormMode ItemFormMode = FormMode.Unknown;
        private InventoryItemPackagePlaceholderToWrite SelectedPlaceholder { get; set; } = new();
        private long ItemId { get; set; } = 0;      // TODO: ItemId won't work for new items since their Ids will all be 0
        private InventoryItemPackagePlaceholderToWrite Placeholder { get; set; } = new();

        private TelerikGrid<InventoryItemPackagePlaceholderToWrite> PlaceholdersGrid { get; set; } = null!;
        private InventoryItemPackagePlaceholderToWrite PlaceholderToAdd { get; set; } = new();
        public InventoryItemToReadInList SelectedInventoryItem { get; set; } = new();
        public List<InventoryItemPackagePlaceholderToWrite> SelectedPlaceholders { get; set; } = null!;
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
            await InitializeNewItemAsPackage();
        }

        private async Task InitializeNewItemAsPackage()
        {
            if (Item?.Package is not null)
                return;

            await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
            await InitializeItemManufacturerProductCode();
            InitializePackage();
            Title = "Add Package";
        }

        private async Task InitializeItemManufacturerProductCode()
        {
            await ProductCodeDataService.GetByManufacturerAsync(Item.Manufacturer.Id)
                .Match(
                    async success =>
                        Item.ProductCode = await GetProductCodeByFirstId(success),
                    failure =>
                        Item.ProductCode = new ProductCodeToRead()
                );
        }
        private async Task<ProductCodeToRead> GetProductCodeByFirstId(IReadOnlyList<ProductCodeToReadInList> productCodes)
        {
            var firstId = productCodes.FirstOrDefault()?.Id ?? 0;
            var productCodeResult = await ProductCodeDataService.GetAsync(firstId);
            return productCodeResult.IsSuccess ? productCodeResult.Value : new ProductCodeToRead();
        }

        private void InitializePackage()
        {
            Item.Package = new InventoryItemPackageToWrite();
            Item.ItemType = InventoryItemType.Package;
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
