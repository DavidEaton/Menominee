using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class SaleCodeListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

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
            SaleCodes = (await SaleCodeDataService.GetAllSaleCodesAsync()).ToList();

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
                var saleCode = await SaleCodeDataService.GetSaleCodeAsync(Id);
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
            Id = (await SaleCodeDataService.AddSaleCodeAsync(SaleCode)).Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            await SaleCodeDataService.UpdateSaleCodeAsync(SaleCode, Id);
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
            SaleCodes = (await SaleCodeDataService.GetAllSaleCodesAsync()).ToList();
            SelectedSaleCode = SaleCodes.Where(x => x.Id == Id).FirstOrDefault();
            SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
        }
    }
}
