using CSharpFunctionalExtensions;
using Menominee.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders.Pages
{
    public partial class RepairOrderListPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRepairOrderDataService DataService { get; set; }

        [Parameter]
        public long ROToSelect { get; set; } = 0;

        //[CascadingParameter]
        //MainLayout mainLayout { get; set; }
        [Inject]
        ILogger<RepairOrderListPage> Logger { get; set; }

        public IReadOnlyList<RepairOrderToReadInList> ROList { get; set; } = new List<RepairOrderToReadInList>();
        public IEnumerable<RepairOrderToReadInList> SelectedList { get; set; } = Enumerable.Empty<RepairOrderToReadInList>();
        public RepairOrderToReadInList SelectedItem { get; set; }

        public TelerikGrid<RepairOrderToReadInList> Grid { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool Loading { get; set; } = true;

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
            await DataService.GetAllAsync()
            .Match(
                success => ROList = success,
                failure => Logger.LogError(failure));

            Loading = false;

            if (ROList?.Any() == true)
            {
                if (ROToSelect == 0)
                {
                    SelectedItem = ROList.FirstOrDefault();
                }
                else
                {
                    SelectedItem = ROList.Where(ro => ro.Id == ROToSelect).FirstOrDefault();
                }
                SelectedId = SelectedItem.Id;
                SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
            }
            else
            {
                SelectedId = 0;
            }
            StateHasChanged();
        }

        private void OnAdd()
        {
            //ToggleEditMenu(true);
            NavigationManager.NavigateTo("repairorders/ro/0");
        }

        private void OnEdit()
        {
            NavigationManager.NavigateTo($"repairorders/ro/{SelectedId}");
        }

        private void OnDelete()
        {
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("repairorders");
        }

        protected void OnSelect(IEnumerable<RepairOrderToReadInList> ros)
        {
            SelectedItem = ros.FirstOrDefault();
            SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as RepairOrderToReadInList).Id;
        }
    }
}
