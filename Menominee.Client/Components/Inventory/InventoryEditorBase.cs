using CSharpFunctionalExtensions;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public abstract class InventoryEditorBase : ComponentBase
    {
        [Inject]
        public IManufacturerDataService ManufacturerDataService { get; set; } = null!;

        [Inject]
        public IProductCodeDataService ProductCodeDataService { get; set; } = null!;

        [Parameter]
        public InventoryItemToWrite Item { get; set; } = new();

        protected string SaleCode { get; set; } = string.Empty;
        protected long ProductCodeId { get; set; } = default;
        protected string Title { get; set; } = string.Empty;

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        protected long ManufacturerId;
        protected IReadOnlyList<ProductCodeToReadInList>? ProductCodes = new List<ProductCodeToReadInList>();
        protected IReadOnlyList<ManufacturerToReadInList> Manufacturers = new List<ManufacturerToReadInList>();

        protected bool ParametersSet = false;
        public InventoryEditorBase()
        {
            ProductCodes = new List<ProductCodeToReadInList>().AsReadOnly();
            Manufacturers = new List<ManufacturerToReadInList>().AsReadOnly();
        }

        protected Task OnProductCodeChangedAsync()
            => ProductCodeChanged(ProductCodeId)
                ? HandleProductCodeChange()
                : Task.CompletedTask;

        private async Task HandleProductCodeChange()
        {
            var result = await GetProductCodeAsync(ProductCodeId);
            var updateResult = UpdateProductCodeAndId(result);
            Item.ProductCode = updateResult.Item;
            Item.ProductCodeId = updateResult.Id;
            SaleCode = GenerateSaleCode(Item);
        }

        protected async Task OnParametersSetCommonAsync(InventoryItemType itemType, string addTitle, string editTitle)
        {
            InitializeParametersSet();
            await OnProductCodeChangedAsync();

            Title = Item.Id == 0 ? addTitle : editTitle;
            Item.ItemType = itemType;
            StateHasChanged();
        }

        private static (ProductCodeToRead Item, long Id) UpdateProductCodeAndId(Result<ProductCodeToRead> result)
        {
            var itemField = result.IsSuccess ? result.Value : new ProductCodeToRead();
            var idField = result.IsSuccess ? result.Value.Id : 0;
            return (itemField, idField);
        }

        protected static string GenerateSaleCode(InventoryItemToWrite Item)
        {
            return Item?.ProductCode is not null
                ? $"{Item.ProductCode.SaleCode.Code} - {Item.ProductCode.SaleCode.Name}"
                : string.Empty;
        }

        protected bool ProductCodeChanged(long id)
        {
            return id > 0 && Item.ProductCode?.Id != id;
        }

        protected void UpdateItemProductCode(Result<ProductCodeToRead> result)
        {
            Item.ProductCode = result.IsSuccess ? result.Value : null;
            Item.ProductCodeId = result.IsSuccess ? result.Value.Id : 0;
        }

        protected async Task<Result<ProductCodeToRead>> GetProductCodeAsync(long id)
            => await ProductCodeDataService.GetAsync(id);

        protected async Task OnManufacturerChangedAsync()
        {
            if (ManufacturerId > 0 && Item.Manufacturer?.Id != ManufacturerId)
                await LoadItemManufacturer(ManufacturerId);

            if (Item?.Manufacturer is not null)
                await LoadAndUpdateProductCodeByManufacturerAsync();
            else
                ResetItemProductCode();

            await OnProductCodeChangedAsync();
        }

        private async Task LoadAndUpdateProductCodeByManufacturerAsync()
        {
            var loadedProductCodeId = ProductCodeId;

            await LoadProductCodesByManufacturer();

            if (loadedProductCodeId > 0 && Item.ProductCode?.Id == 0 && ProductCodes.Any(pc => pc.Id == loadedProductCodeId))
            {
                await ProductCodeDataService.GetAsync(loadedProductCodeId)
                .Match(
                    success => Item.ProductCode = success,
                    failure => Item.Manufacturer = new()
                );
            }

            ProductCodeId = loadedProductCodeId;
        }

        protected void ResetItemProductCode()
        {
            ProductCodeId = 0;
            ProductCodes = new List<ProductCodeToReadInList>();
            Item.ProductCode = new();
        }

        protected async Task LoadProductCodesByManufacturer()
        {
            await ProductCodeDataService.GetByManufacturerAsync(ManufacturerId)
                .Match(
                    success => ProductCodes = success,
                    failure => ProductCodes = new List<ProductCodeToReadInList>());
        }

        protected void InitializeParametersSet()
        {
            if (ParametersSet)
                return;

            ParametersSet = true;

            SetManufacturerId();
            SetProductCodeId();

        }

        private void SetManufacturerId()
        {
            ManufacturerId =
                Item?.Manufacturer is not null
                ? Item.Manufacturer.Id
                : default;
        }

        protected async Task LoadSaleCodesByManufacturerAsync()
        {
            await ProductCodeDataService.GetByManufacturerAsync(ManufacturerId)
                .Match(
                    success => ProductCodes = success,
                    failure => ProductCodes = new List<ProductCodeToReadInList>());
        }

        protected void SetProductCodeId()
        {
            ProductCodeId =
                Item?.ProductCode is not null
                    ? Item.ProductCode.Id
                    : default;
        }

        protected static IReadOnlyList<ManufacturerToReadInList> SortManufacturers(IReadOnlyList<ManufacturerToReadInList> filteredManufacturers)
        {
            return filteredManufacturers.OrderBy(manufacturer => manufacturer.Prefix).ToList();
        }

        protected async Task<IReadOnlyList<ManufacturerToReadInList>> GetAllManufacturersAsync()
        {
            return await ManufacturerDataService.GetAllAsync()
                .Match(
                    success => success,
                    failure => new List<ManufacturerToReadInList>()
                );
        }

        protected static IReadOnlyList<ManufacturerToReadInList> FilterManufacturers(IReadOnlyList<ManufacturerToReadInList> allManufacturers)
        {
            return allManufacturers.Where(
                    manufacturer => manufacturer.Prefix?.Length > 0
                    && manufacturer.Id != StaticManufacturerCodes.Custom
                    && manufacturer.Id != StaticManufacturerCodes.Package)
                .ToList();
        }

        protected async Task LoadManufacturers()
        {
            var allManufacturers = await GetAllManufacturersAsync();
            Manufacturers = SortManufacturers(FilterManufacturers(allManufacturers));
        }

        protected async Task LoadItemManufacturer(string code)
        {
            var result = ManufacturerDataService.GetAsync(code);
            await LoadManufacturerAsync(result);
        }

        protected async Task LoadItemManufacturer(long id)
        {
            var result = ManufacturerDataService.GetAsync(id);
            await LoadManufacturerAsync(result);
        }

        private async Task LoadManufacturerAsync(Task<Result<ManufacturerToRead>> manufacturerTask)
        {
            await manufacturerTask
                .Match(
                    success => Item.Manufacturer = success,
                    failure => Item.Manufacturer = new ManufacturerToRead()
                );
        }

        // TODO: Improve method name
        protected async Task LoadItemManufacturerByMiscellaneousStaticManufacturerCode()
        {
            await ManufacturerDataService.GetAsync(StaticManufacturerCodes.Miscellaneous)
                .Match(
                    success =>
                    {
                        Item.Manufacturer = success;
                        ManufacturerId = success.Id;
                    },
                    failure => { /*TODO: log the failure*/ }
                    );
        }
    }
}
