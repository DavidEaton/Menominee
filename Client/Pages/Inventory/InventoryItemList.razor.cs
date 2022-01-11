using MenomineePlayWASM.Shared.Dtos.Inventory;
using MenomineePlayWASM.Shared.Services.Inventory;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Pages.Inventory
{
    public partial class InventoryItemList : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;

        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();
        public InventoryItemToReadInList SelectedItem { get; set; }

        public TelerikGrid<InventoryItemToReadInList> Grid { get; set; }

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
            ItemsList = (await DataService.GetAllItems()).ToList();

            if (ItemsList.Count > 0)
            {
                if (ItemToSelect == 0)
                {
                    SelectedItem = ItemsList.FirstOrDefault();
                }
                else
                {
                    SelectedItem = ItemsList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
                }
                SelectedId = SelectedItem.Id;
                SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            }
            else
            {
                SelectedId = 0;
            }
        }

        private void OnAdd()
        {
            NavigationManager.NavigateTo("inventory/items/0");
        }

        private void OnEdit()
        {
            NavigationManager.NavigateTo($"inventory/items/{SelectedId}");
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
    }
}
