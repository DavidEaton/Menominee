using CSharpFunctionalExtensions;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemSelectForm : ComponentBase
    {
        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Inject]
        public IManufacturerDataService ManufacturerDataService { get; set; }

        [Inject]
        public ILogger<InventoryItemSelectForm> Logger { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public EventCallback OnSelect { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public InventoryItemToReadInList SelectedItem { get; set; }

        [Parameter]
        public EventCallback<InventoryItemToReadInList> SelectedItemChanged { get; set; }

        //[Parameter]
        //public List<InventoryItemType> ExcludedItemTypes { get; set; }

        [Parameter]
        public bool FilterPackagableItems { get; set; } = false;

        [Parameter]
        public bool FilterInstallableItems { get; set; } = false;

        Predicate<InventoryItemToReadInList> itemIsPackagable = ItemMatchesPackageFilter;
        Predicate<InventoryItemToReadInList> itemIsInstallable = ItemMatchesInstallableFilter;

        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();
        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private List<ManufacturerX> ManufacturerList = new();
        private List<string> SearchFields = new();
        private long SelectedMfrId { get; set; } = 0;
        private long ViewingMfrId { get; set; } = 0;

        private bool CanSelect { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            SearchFields.Add("PartNumber");
            SearchFields.Add("Description");

            await FilterItemsList(0);
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
            if (FilterPackagableItems)
            {
                if (mfrId > 0)
                    ItemsList = (await DataService.GetByManufacturerAsync(mfrId)).Value.Where(i => itemIsPackagable(i)).ToList();
                else
                    ItemsList = (await DataService.GetAllAsync()).Value.Where(i => itemIsPackagable(i)).ToList();
            }
            else if (FilterInstallableItems)
            {
                if (mfrId > 0)
                    ItemsList = (await DataService.GetByManufacturerAsync(mfrId)).Value.Where(i => itemIsInstallable(i)).ToList();
                else
                    ItemsList = (await DataService.GetAllAsync()).Value.Where(i => itemIsInstallable(i)).ToList();
            }
            else
            {
                if (mfrId > 0)
                    ItemsList = (await DataService.GetByManufacturerAsync(mfrId)).Value.ToList();
                else
                    ItemsList = (await DataService.GetAllAsync()).Value.ToList();
            }

            if (ItemsList.Count > 0)
            {
                SelectedItem = ItemsList.FirstOrDefault();
                await SelectedItemChanged.InvokeAsync(SelectedItem);
                SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            }

            CanSelect = ItemsList.Count > 0;
        }

        // TODO: Need a better way to do customized filtering
        private static bool ItemMatchesPackageFilter(InventoryItemToReadInList item)
        {
            return item.ItemType != InventoryItemType.Package
                && item.ItemType != InventoryItemType.GiftCertificate
                && item.ItemType != InventoryItemType.Donation;
        }

        private static bool ItemMatchesInstallableFilter(InventoryItemToReadInList item)
        {
            return item.ItemType == InventoryItemType.Part
                || item.ItemType == InventoryItemType.Tire;
        }


        private async Task OnSelectMfr()
        {
            if (SelectedMfrId != ViewingMfrId)
            {
                ViewingMfrId = SelectedMfrId;
                await FilterItemsList(ViewingMfrId);
            }
        }

        protected async Task OnSelectItemAsync(IEnumerable<InventoryItemToReadInList> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            //SelectedId = SelectedItem.Id;
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }

        private async Task OnRowSelectedAsync(GridRowClickEventArgs args)
        {
            //SelectedId = (args.Item as InventoryItemToReadInList).Id;
            SelectedItem = args.Item as InventoryItemToReadInList;
            await SelectedItemChanged.InvokeAsync(SelectedItem);
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
