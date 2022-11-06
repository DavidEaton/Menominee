using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IManufacturerDataService MfrDataService { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;


        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();
        public InventoryItemToReadInList SelectedItem { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private List<ManufacturerX> ManufacturerList = new List<ManufacturerX>();
        private List<string> SearchFields = new List<string>();
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

            //ItemsList = (await DataService.GetAllItems()).ToList();

            //if (ItemsList.Count > 0)
            //{
            //    if (ItemToSelect == 0)
            //    {
            //        SelectedItem = ItemsList.FirstOrDefault();
            //    }
            //    else
            //    {
            //        SelectedItem = ItemsList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
            //    }
            //    SelectedId = SelectedItem.Id;
            //    SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            //}
            //else
            //{
            //    SelectedId = 0;
            //}
        }

        protected override async Task OnParametersSetAsync()
        {
            Manufacturers = (await MfrDataService.GetAllManufacturersAsync()).ToList();

            ManufacturerList = new();
            ManufacturerList.Add(new ManufacturerX
            {
                Id = 0,
                Code = "",
                Prefix = "",
                Name = "<< All >>"
            });
            foreach (var mfr in Manufacturers)
            {
                if (mfr.Code != "0" && mfr.Prefix?.Length > 0)       // FIX ME - need server to only return list of configured Mfrs
                {
                    ManufacturerList.Add(new ManufacturerX
                    {
                        Id = mfr.Id,
                        Code = mfr.Code,
                        Prefix = mfr.Prefix,
                        Name = mfr.Name
                    });
                }
            }
        }

        private async Task FilterItemsList(long mfrId)
        {
            if (mfrId > 0)
                ItemsList = (await DataService.GetAllItemsAsync(mfrId)).ToList();
            else
                ItemsList = (await DataService.GetAllItemsAsync()).ToList();

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

        private string ItemTypeUrlSegment(InventoryItemType itemType)
        {
            string url = string.Empty;

            if (itemType == InventoryItemType.Part)
                url = "parts";
            else if (itemType == InventoryItemType.Labor)
                url = "labor";
            else if (itemType == InventoryItemType.Tire)
                url = "tires";
            else if (itemType == InventoryItemType.Package)
                url = "packages";
            else if (itemType == InventoryItemType.Inspection)
                url = "inspections";
            else if (itemType == InventoryItemType.Donation)
                url = "donations";
            else if (itemType == InventoryItemType.GiftCertificate)
                url = "giftcertificates";
            else if (itemType == InventoryItemType.Warranty)
                url = "warranties";

            return url;
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
            string url = ItemTypeUrlSegment(Utilities.ParseEnum<InventoryItemType>(SelectedItem.ItemType)) ?? string.Empty;
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