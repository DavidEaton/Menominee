using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class SalesTaxListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISalesTaxDataService SalesTaxDataService { get; set; }

        public IReadOnlyList<SalesTaxToReadInList> SalesTaxes;
        public IEnumerable<SalesTaxToReadInList> SelectedSalesTaxes { get; set; } = Enumerable.Empty<SalesTaxToReadInList>();
        public SalesTaxToReadInList SelectedSalesTax { get; set; }
        public SalesTaxToWrite SalesTax { get; set; } = null;
        public TelerikGrid<SalesTaxToReadInList> Grid { get; set; }

        [Parameter]
        public long Id { get; set; } = 0;

        private bool CanEdit { get => Id > 0; }
        private bool CanDelete { get => Id > 0; }

        private FormMode SalesTaxFormMode = FormMode.Unknown;

        protected override async Task OnInitializedAsync()
        {
            await GetSalesTaxes();

            if (SalesTaxes?.Count > 0)
            {
                SelectedSalesTax = SalesTaxes.FirstOrDefault();
                Id = SelectedSalesTax.Id;
                SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
            }
        }

        private void OnAdd()
        {
            //Id = 0;
            SalesTaxFormMode = FormMode.Add;
            //SalesTaxes = null;
            SalesTax = new();
            SalesTax.IsAppliedByDefault = true;
            SalesTax.TaxType = SalesTaxType.Normal;
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as SalesTaxToReadInList).Id;
            await OnEditAsync();
        }

        private async Task OnEditAsync()
        {
            if (Id > 0)
            {
                var result = await SalesTaxDataService.GetAsync(Id);

                SalesTax = result.IsSuccess
                    ? new SalesTaxToWrite
                    {
                        Description = result.Value.Description,
                        IsAppliedByDefault = result.Value.IsAppliedByDefault,
                        IsTaxable = result.Value.IsTaxable,
                        LaborTaxRate = result.Value.LaborTaxRate,
                        Order = result.Value.Order,
                        PartTaxRate = result.Value.PartTaxRate,
                        TaxIdNumber = result.Value.TaxIdNumber,
                        TaxType = result.Value.TaxType,
                        ExciseFees = result.Value.ExciseFees
                        ?.Select(
                            tax => new ExciseFeeToWrite
                            {
                                Amount = tax.Amount,
                                Description = tax.Description,
                                FeeType = tax.FeeType
                            })
                        .ToList()
                        ?? new List<ExciseFeeToWrite>()
                    }
                    : new SalesTaxToWrite();

                SalesTaxFormMode = FormMode.Edit;
            }
        }

        private void OnDelete()
        {
            //if (Id > 0)
            //    await SalesTaxDataService.Delete(Id);
        }

        protected void OnSelect(IEnumerable<SalesTaxToReadInList> salesTaxes)
        {
            SelectedSalesTax = salesTaxes.FirstOrDefault();
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as SalesTaxToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        protected async Task HandleAddSubmitAsync()
        {
            Id = (await SalesTaxDataService.AddAsync(SalesTax)).Value.Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            await SalesTaxDataService.UpdateAsync(SalesTax);
            await EndAddEditAsync();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (SalesTaxFormMode == FormMode.Add)
                await HandleAddSubmitAsync();
            else if (SalesTaxFormMode == FormMode.Edit)
                await HandleEditSubmitAsync();
        }

        protected async Task EndAddEditAsync()
        {
            SalesTaxFormMode = FormMode.Unknown;
            await GetSalesTaxes();

            SelectedSalesTax = SalesTaxes.Where(x => x.Id == Id).FirstOrDefault();
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }

        private async Task GetSalesTaxes()
        {
            var result = await SalesTaxDataService.GetAllAsync();

            if (result.IsSuccess)
                SalesTaxes = (IReadOnlyList<SalesTaxToReadInList>)result.Value.OrderByDescending(tax => tax.Order);
        }
    }
}
