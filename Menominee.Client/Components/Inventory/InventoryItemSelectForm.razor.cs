using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemSelectForm : ComponentBase
    {
        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Inject]
        public IManufacturerDataService MfrDataService { get; set; }

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

        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();
        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private List<ManufacturerX> ManufacturerList = new List<ManufacturerX>();
        private List<string> SearchFields = new List<string>();
        private long SelectedMfrId { get; set; } = 0;
        private long ViewingMfrId { get; set; } = 0;

        private bool CanSelect { get; set; } = false;

        //private long selectedId = 0;
        //public long SelectedId
        //{
        //    get => selectedId;
        //    set
        //    {
        //        selectedId = value;
        //        CanSelect = selectedId > 0;
        //    }
        //}

        protected override async Task OnInitializedAsync()
        {
            SearchFields.Add("PartNumber");
            SearchFields.Add("Description");

            await FilterItemsList(0);
            //ItemsList = (await DataService.GetAllItems()).ToList();

            //if (ItemsList.Count > 0)
            //{
            //    SelectedItem = ItemsList.FirstOrDefault();
            //    await SelectedItemChanged.InvokeAsync(SelectedItem);
            //    //SelectedId = SelectedItem.Id;
            //    SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            //}
            //else
            //{
            //    //SelectedId = 0;
            //}

            //CanSelect = ItemsList.Count > 0;
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
                if (mfr.Code != "0" && mfr.Prefix.Length > 0)       // FIX ME - need server to only return list of configured Mfrs
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
                SelectedItem = ItemsList.FirstOrDefault();
                await SelectedItemChanged.InvokeAsync(SelectedItem);
                SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            }

            CanSelect = ItemsList.Count > 0;
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
