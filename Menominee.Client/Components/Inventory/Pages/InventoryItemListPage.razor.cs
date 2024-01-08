using CSharpFunctionalExtensions;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory.Pages
{
    public partial class InventoryItemListPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Inject]
        public IManufacturerDataService ManufacturerDataService { get; set; }

        [Inject]
        public ILogger<InventoryItemListPage> Logger { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;


        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();
        public InventoryItemToReadInList SelectedItem { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private List<ManufacturerX> ManufacturerList = new();
        private List<string> SearchFields = new();
        private long SelectedMfrId { get; set; } = 0;
        private long ViewingMfrId { get; set; } = 0;

        public TelerikGrid<InventoryItemToReadInList> Grid { get; set; }

        private bool ItemTypeSelectDialogVisible { get; set; } = false;
        private InventoryItemType SelectedItemType;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        private long selectedId = 0;
        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId > 0;
                CanDelete = selectedId > 0;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            SearchFields.Add("ItemNumber");
            SearchFields.Add("Description");

            await FilterItemsList(0);

            SelectedItemType = InventoryItemType.Part;

            ItemsList = (await DataService.GetAllAsync()).Value.ToList();

            SelectedItem = ItemsList.DefaultIfEmpty(new InventoryItemToReadInList { Id = 0 })
                                     .FirstOrDefault(item => ItemToSelect == 0 || item.Id == ItemToSelect);

            SelectedId = SelectedItem.Id;
            SelectedList = new List<InventoryItemToReadInList> { SelectedItem };

        }

        protected override async Task OnParametersSetAsync()
        {
            await ManufacturerDataService.GetAllAsync()
                .Match(
                    success =>
                    {
                        Manufacturers = success;
                    },
                    failure => Logger.LogError(failure)
                );

            ManufacturerList = new()
            {
                new()
                {
                    Id = 0,
                    Code = "",
                    Prefix = "",
                    Name = "<< All >>"
                }
            };

            foreach (var mfr in Manufacturers)
            {
                if (mfr.Id != 0 && mfr.Prefix?.Length > 0)       // FIX ME - need server to only return list of configured Mfrs
                {
                    ManufacturerList.Add(new ManufacturerX
                    {
                        Id = mfr.Id,
                        Prefix = mfr.Prefix,
                        Name = mfr.Name
                    });
                }
            }
        }

        private async Task FilterItemsList(long mfrId)
        {
            if (mfrId > 0)
                ItemsList = (await DataService.GetByManufacturerAsync(mfrId)).Value.ToList();
            else
                ItemsList = (await DataService.GetAllAsync()).Value.ToList();

            if (ItemsList.Count > 0)
            {
                if (ItemToSelect == 0)
                    SelectedItem = ItemsList.FirstOrDefault();
                else
                    SelectedItem = ItemsList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
                SelectedId = SelectedItem.Id;
                SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            }
            else
            {
                SelectedId = 0;
            }

            //CanSelect = ItemsList.Count > 0;
        }

        private async Task OnSelectMfr()
        {
            if (SelectedMfrId != ViewingMfrId)
            {
                ViewingMfrId = SelectedMfrId;
                await FilterItemsList(ViewingMfrId);
                Grid.Rebind();
            }
        }

        private void OnAdd()
        {
            ItemTypeSelectDialogVisible = true;
        }

        private static string ItemTypeUrlSegment(InventoryItemType itemType)
        {
            return itemType switch
            {
                InventoryItemType.Part => "parts",
                InventoryItemType.Labor => "labor",
                InventoryItemType.Tire => "tires",
                InventoryItemType.Package => "packages",
                InventoryItemType.Inspection => "inspections",
                InventoryItemType.Donation => "donations",
                InventoryItemType.GiftCertificate => "giftcertificates",
                InventoryItemType.Warranty => "warranties",
                _ => string.Empty,
            };
        }

        private void OnSelectItemType()
        {
            ItemTypeSelectDialogVisible = false;

            string url = ItemTypeUrlSegment(SelectedItemType) ?? string.Empty;
            if (url.Length > 0)
                NavigationManager.NavigateTo($"inventory/{url}/0");
        }

        private void OnEdit()
        {
            string url = ItemTypeUrlSegment(SelectedItem.ItemType);
            if (url.Length > 0)
                NavigationManager.NavigateTo($"inventory/{url}/{SelectedId}");
        }

        private void OnDelete()
        {
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("inventory");
        }

        protected void OnSelect(IEnumerable<InventoryItemToReadInList> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as InventoryItemToReadInList).Id;
        }

        public class ManufacturerX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Prefix { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return (Prefix.Length > 0) ? (Prefix + " - " + Name) : Name;
                }
            }
        }
    }
}