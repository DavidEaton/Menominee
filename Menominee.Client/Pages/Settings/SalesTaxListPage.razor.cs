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
    public partial class SalesTaxListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISalesTaxDataService SalesTaxDataService { get; set; }

        [Parameter]
        public long TaxToSelect { get; set; } = 0;

        public IReadOnlyList<SalesTaxToReadInList> SalesTaxList;
        public IEnumerable<SalesTaxToReadInList> SelectedSalesTaxes { get; set; } = Enumerable.Empty<SalesTaxToReadInList>();
        public SalesTaxToReadInList SelectedSalesTax { get; set; }
        public SalesTaxToWrite SalesTaxToModify { get; set; } = null;

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

        public TelerikGrid<SalesTaxToReadInList> Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool EditTaxDialogVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            SalesTaxList = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();

            if (SalesTaxList.Count > 0)
            {
                if (TaxToSelect == 0)
                {
                    SelectedSalesTax = SalesTaxList.FirstOrDefault();
                }
                else
                {
                    SelectedSalesTax = SalesTaxList.Where(x => x.Id == TaxToSelect).FirstOrDefault();
                }
                SelectedId = SelectedSalesTax.Id;
                SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            GridState<SalesTaxToReadInList> desiredState = new GridState<SalesTaxToReadInList>()
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
            SalesTaxToModify = new();
            SalesTaxToModify.TaxType = SalesTaxType.Normal;
            EditTaxDialogVisible = true;
        }

        private async Task OnEditAsync()
        {
            if (SelectedId != 0)
            {
                SalesTaxToRead tax = await SalesTaxDataService.GetSalesTaxAsync(SelectedId);
                if (tax != null)
                {
                    SalesTaxToModify = SalesTaxHelper.Transform(tax);
                    EditTaxDialogVisible = true;
                }
            }
        }

        private void OnDelete()
        {
            SelectedId = 0;
        }

        protected void OnSelect(IEnumerable<SalesTaxToReadInList> SalesTaxs)
        {
            SelectedSalesTax = SalesTaxs.FirstOrDefault();
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as SalesTaxToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task OnSaveEditAsync()
        {
            if (SelectedId == 0)
            {
                SalesTaxToRead tax = await SalesTaxDataService.AddSalesTaxAsync(SalesTaxToModify);
                if (tax != null)
                {
                    SelectedId = tax.Id;
                }
            }
            else
            {
                await SalesTaxDataService.UpdateSalesTaxAsync(SelectedId, SalesTaxToModify);
            }

            if (SelectedId > 0)
            {
                SalesTaxList = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
                SelectedSalesTax = SalesTaxList.Where(x => x.Id == SelectedId).FirstOrDefault();
                SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
                Grid.Rebind();
            }

            EditTaxDialogVisible = false;
        }

        private void OnCancelEdit()
        {
            EditTaxDialogVisible = false;
        }
    }
}
