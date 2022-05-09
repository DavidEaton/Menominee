using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace Menominee.Client.Pages.Settings
{
    public partial class ExciseFeeListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IExciseFeeDataService ExciseFeeDataService { get; set; }

        [Parameter]
        public long FeeToSelect { get; set; } = 0;

        public IReadOnlyList<ExciseFeeToReadInList> ExciseFeeList;
        public IEnumerable<ExciseFeeToReadInList> SelectedExciseFees { get; set; } = Enumerable.Empty<ExciseFeeToReadInList>();
        public ExciseFeeToReadInList SelectedExciseFee { get; set; }
        public ExciseFeeToWrite ExciseFeeToModify { get; set; } = null;

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

        public TelerikGrid<ExciseFeeToReadInList> Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool EditFeeDialogVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            ExciseFeeList = (await ExciseFeeDataService.GetAllExciseFeesAsync()).ToList();

            if (ExciseFeeList.Count > 0)
            {
                if (FeeToSelect == 0)
                {
                    SelectedExciseFee = ExciseFeeList.FirstOrDefault();
                }
                else
                {
                    SelectedExciseFee = ExciseFeeList.Where(x => x.Id == FeeToSelect).FirstOrDefault();
                }
                SelectedId = SelectedExciseFee.Id;
                SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            GridState<ExciseFeeToReadInList> desiredState = new GridState<ExciseFeeToReadInList>()
            {
                SortDescriptors = new List<SortDescriptor>()
                {
                    new SortDescriptor { Member = "Name", SortDirection = ListSortDirection.Descending }
                }
            };

            if (Grid != null)
                await Grid?.SetState(desiredState);
        }

        private void OnAdd()
        {
            SelectedId = 0;
            ExciseFeeToModify = new();
            ExciseFeeToModify.FeeType = ExciseFeeType.Flat;
            EditFeeDialogVisible = true;
        }

        private async Task OnEditAsync()
        {
            if (SelectedId != 0)
            {
                ExciseFeeToRead fee = await ExciseFeeDataService.GetExciseFeeAsync(SelectedId);
                if (fee != null)
                {
                    ExciseFeeToModify = ExciseFeeHelper.CreateExciseFee(fee);
                    EditFeeDialogVisible = true;
                }
            }
        }

        private void OnDelete()
        {
            SelectedId = 0;
        }

        protected void OnSelect(IEnumerable<ExciseFeeToReadInList> ExciseFees)
        {
            SelectedExciseFee = ExciseFees.FirstOrDefault();
            SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as ExciseFeeToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task OnSaveEditAsync()
        {
            if (SelectedId == 0)
            {
                ExciseFeeToRead fee = await ExciseFeeDataService.AddExciseFeeAsync(ExciseFeeToModify);
                if (fee != null)
                {
                    SelectedId = fee.Id;
                }
            }
            else
            {
                await ExciseFeeDataService.UpdateExciseFeeAsync(SelectedId, ExciseFeeToModify);
            }

            if (SelectedId > 0)
            {
                ExciseFeeList = (await ExciseFeeDataService.GetAllExciseFeesAsync()).ToList();
                SelectedExciseFee = ExciseFeeList.Where(x => x.Id == SelectedId).FirstOrDefault();
                SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
                Grid.Rebind();
            }

            EditFeeDialogVisible = false;
        }

        private void OnCancelEdit()
        {
            EditFeeDialogVisible = false;
        }
    }
}
