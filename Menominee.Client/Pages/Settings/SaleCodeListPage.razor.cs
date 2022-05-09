using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.SaleCodes;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace Menominee.Client.Pages.Settings
{
    public partial class SaleCodeListPage
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        [Parameter]
        public long CodeToSelect { get; set; } = 0;

        public IReadOnlyList<SaleCodeToReadInList> SaleCodeList;
        public IEnumerable<SaleCodeToReadInList> SelectedSaleCodes { get; set; } = Enumerable.Empty<SaleCodeToReadInList>();
        public SaleCodeToReadInList SelectedSaleCode { get; set; }
        public SaleCodeToWrite SaleCodeToModify { get; set; } = null;

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

        public TelerikGrid<SaleCodeToReadInList> Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool EditCodeDialogVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            SaleCodeList = (await SaleCodeDataService.GetAllSaleCodesAsync()).ToList();

            if (SaleCodeList.Count > 0)
            {
                if (CodeToSelect == 0)
                {
                    SelectedSaleCode = SaleCodeList.FirstOrDefault();
                }
                else
                {
                    SelectedSaleCode = SaleCodeList.Where(x => x.Id == CodeToSelect).FirstOrDefault();
                }
                SelectedId = SelectedSaleCode.Id;
                SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            GridState<SaleCodeToReadInList> desiredState = new GridState<SaleCodeToReadInList>()
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
            SaleCodeToModify = new();
            EditCodeDialogVisible = true;
        }

        private async Task OnEditAsync()
        {
            if (SelectedId != 0)
            {
                SaleCodeToRead sc = await SaleCodeDataService.GetSaleCodeAsync(SelectedId);
                if (sc != null)
                {
                    SaleCodeToModify = SaleCodeHelper.CreateSaleCode(sc);
                    EditCodeDialogVisible = true;
                }
            }
        }

        private void OnDelete()
        {
            SelectedId = 0;
        }

        protected void OnSelect(IEnumerable<SaleCodeToReadInList> saleCodes)
        {
            SelectedSaleCode = saleCodes.FirstOrDefault();
            SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as SaleCodeToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task OnSaveEditAsync()
        {
            if (SelectedId == 0)
            {
                SaleCodeToRead sc = await SaleCodeDataService.AddSaleCodeAsync(SaleCodeToModify);
                if (sc != null)
                {
                    SelectedId = sc.Id;
                }
            }
            else
            {
                await SaleCodeDataService.UpdateSaleCodeAsync(SaleCodeToModify, SelectedId);
            }

            if (SelectedId > 0)
            {
                SaleCodeList = (await SaleCodeDataService.GetAllSaleCodesAsync()).ToList();
                SelectedSaleCode = SaleCodeList.Where(x => x.Id == SelectedId).FirstOrDefault();
                SelectedSaleCodes = new List<SaleCodeToReadInList> { SelectedSaleCode };
                Grid.Rebind();
            }

            EditCodeDialogVisible = false;
        }

        private void OnCancelEdit()
        {
            EditCodeDialogVisible = false;
        }
    }
}
