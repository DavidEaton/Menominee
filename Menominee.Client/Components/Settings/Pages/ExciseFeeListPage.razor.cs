using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class ExciseFeeListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IExciseFeeDataService ExciseFeeDataService { get; set; }

        [Parameter]
        public long Id { get; set; } = 0;

        public IReadOnlyList<ExciseFeeToReadInList> ExciseFees;
        public IEnumerable<ExciseFeeToReadInList> SelectedExciseFees { get; set; } = Enumerable.Empty<ExciseFeeToReadInList>();
        public ExciseFeeToReadInList SelectedExciseFee { get; set; }
        public ExciseFeeToWrite ExciseFee { get; set; } = null;

        public TelerikGrid<ExciseFeeToReadInList> Grid { get; set; }

        private bool CanEdit { get => Id > 0; }
        private bool CanDelete { get => Id > 0; }

        private FormMode EditFormMode = FormMode.Unknown;

        protected override async Task OnInitializedAsync()
        {
            ExciseFees = (await ExciseFeeDataService.GetAllAsync()).Value.ToList();

            if (ExciseFees?.Count > 0)
            {
                SelectedExciseFee = ExciseFees.FirstOrDefault();
                Id = SelectedExciseFee.Id;
                SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
            }
        }

        // TODO - Failed attempt to sort by name initially
        //protected override async Task OnParametersSetAsync()
        //{
        //    GridState<ExciseFeeToReadInList> desiredState = new GridState<ExciseFeeToReadInList>()
        //    {
        //        SortDescriptors = new List<SortDescriptor>()
        //        {
        //            new SortDescriptor { Member = "Name", SortDirection = ListSortDirection.Descending }
        //        }
        //    };

        //    if (Grid != null)
        //        await Grid?.SetState(desiredState);
        //}

        private void OnAdd()
        {
            //Id = 0;
            EditFormMode = FormMode.Add;
            //ExciseFees = null;
            ExciseFee = new();
            ExciseFee.FeeType = ExciseFeeType.Flat;
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as ExciseFeeToReadInList).Id;
            await OnEditAsync();
        }

        private async Task OnEditAsync()
        {
            if (Id != 0)
            {
                var result = await ExciseFeeDataService.GetAsync(Id);

                if (result.IsSuccess)
                    ExciseFee = ExciseFeeHelper.ConvertReadToWriteDto(result.Value);

                if (result.IsFailure)
                {
                    // TODO: Al, do we just create a new empty ExiceFeeToRead dto?
                    // Do we need to log this?, Let user know?
                    ExciseFee = new();
                }

                EditFormMode = FormMode.Edit;
            }
        }

        private void OnDelete()
        {
            //if (Id > 0)
            //    await ExciseFeeDataService.Delete(Id);
        }

        protected void OnSelect(IEnumerable<ExciseFeeToReadInList> ExciseFees)
        {
            SelectedExciseFee = ExciseFees.FirstOrDefault();
            SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as ExciseFeeToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        protected async Task HandleAddSubmitAsync()
        {
            Id = (await ExciseFeeDataService.AddAsync(ExciseFee)).Value.Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            await ExciseFeeDataService.UpdateAsync(ExciseFee);
            await EndAddEditAsync();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (EditFormMode == FormMode.Add)
                await HandleAddSubmitAsync();
            else if (EditFormMode == FormMode.Edit)
                await HandleEditSubmitAsync();
        }

        protected async Task EndAddEditAsync()
        {
            EditFormMode = FormMode.Unknown;
            ExciseFees = (await ExciseFeeDataService.GetAllAsync()).Value.ToList();
            SelectedExciseFee = ExciseFees.Where(x => x.Id == Id).FirstOrDefault();
            SelectedExciseFees = new List<ExciseFeeToReadInList> { SelectedExciseFee };
        }
    }
}
