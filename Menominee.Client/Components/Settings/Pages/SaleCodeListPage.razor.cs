using Blazored.Toast.Services;
using Menominee.Client.Services.SaleCodes;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class SaleCodeListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        [Parameter]
        public long Id { get; set; } = 0;

        public IReadOnlyList<SaleCodeToReadInList> SaleCodes;
        public IEnumerable<SaleCodeToReadInList> SelectedSaleCodes { get; set; } = Enumerable.Empty<SaleCodeToReadInList>();
        public SaleCodeToReadInList SelectedSaleCode { get; set; }
        public SaleCodeToWrite SaleCode { get; set; } = null;
        public TelerikGrid<SaleCodeToReadInList> Grid { get; set; }


        private bool CanEdit { get => Id > 0; }
        private bool CanDelete { get => Id > 0; }

        private FormMode EditFormMode = FormMode.Unknown;

        protected override async Task OnInitializedAsync()
        {
            SaleCodes = (await SaleCodeDataService.GetAllAsync()).ToList();

            if (SaleCodes?.Count > 0)
            {
                SelectedSaleCode = SaleCodes.FirstOrDefault();
                Id = SelectedSaleCode.Id;
                SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
            }
        }

        private void OnAdd()
        {
            //Id = 0;
            SaleCode = new();
            EditFormMode = FormMode.Add;
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as SaleCodeToReadInList).Id;
            await OnEditAsync();
        }

        private async Task OnEditAsync()
        {
            if (Id != 0)
            {
                var saleCode = await SaleCodeDataService.GetAsync(Id);
                if (saleCode != null)
                {
                    SaleCode = SaleCodeHelper.ConvertReadToWriteDto(saleCode);
                    EditFormMode = FormMode.Edit;
                }
            }
        }

        private void OnDelete()
        {
            //if (Id > 0)
            //    await SaleCodeDataService.Delete(Id);
        }

        protected void OnSelect(IEnumerable<SaleCodeToReadInList> saleCodes)
        {
            SelectedSaleCode = saleCodes.FirstOrDefault();
            SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as SaleCodeToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        protected async Task HandleAddSubmitAsync()
        {
            var response = await SaleCodeDataService.AddAsync(SaleCode);

            if (response.IsFailure)
            {
                ToastService.ShowError(response.Error);
                return;
            }

            Id = response.Value.Id;

            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            var response = await SaleCodeDataService.UpdateAsync(SaleCode);

            if (response.IsFailure)
            {
                ToastService.ShowError(response.Error);
                return;
            }

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
            SaleCodes = (await SaleCodeDataService.GetAllAsync()).ToList();
            SelectedSaleCode = SaleCodes.Where(x => x.Id == Id).FirstOrDefault();
            SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
        }
    }
}
