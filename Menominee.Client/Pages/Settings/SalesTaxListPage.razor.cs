using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Settings
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
            SalesTaxes = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
            if (SalesTaxes?.Count > 0)
            {
                SelectedSalesTax = SalesTaxes.FirstOrDefault();
                Id = SelectedSalesTax.Id;
                SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
            }
        }

        private void OnAdd()
        {
            Id = 0;
            SalesTaxFormMode = FormMode.Add;
            SalesTaxes = null;
            SalesTax = new();
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as SalesTaxToReadInList).Id;
            await Edit();
        }

        private async Task Edit()
        {
            if (Id > 0)
            {
                var readDto = await SalesTaxDataService.GetSalesTaxAsync(Id);
                SalesTax = new SalesTaxToWrite
                {
                    Description = readDto.Description,
                    IsAppliedByDefault = readDto.IsAppliedByDefault,
                    IsTaxable = readDto.IsTaxable,
                    LaborTaxRate = readDto.LaborTaxRate,
                    Order = readDto.Order,
                    PartTaxRate = readDto.PartTaxRate,
                    TaxIdNumber = readDto.TaxIdNumber,
                    TaxType = readDto.TaxType
                };

                if (readDto?.TaxedExciseFees.Count > 0)
                {
                    foreach (var tax in readDto.TaxedExciseFees)
                    {
                        SalesTax.TaxedExciseFees.Add(new ExciseFeeToWrite
                        {
                            Amount = tax.Amount,
                            Description = tax.Description,
                            FeeType = tax.FeeType
                        });
                    }
                }

                SalesTaxFormMode = FormMode.Edit;
                SalesTaxes = null;
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

        protected async Task HandleAddSubmit()
        {
            //await SalesTaxDataService.AddSalesTaxAsync(SalesTax);
            SalesTaxToRead tax = await SalesTaxDataService.AddSalesTaxAsync(SalesTax);
            Id = tax.Id;
            await EndAddEditAsync();
        }

        protected async Task HandleEditSubmit()
        {
            await SalesTaxDataService.UpdateSalesTaxAsync(Id, SalesTax);
            await EndAddEditAsync();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (SalesTaxFormMode == FormMode.Add)
                await HandleAddSubmit();

            if (SalesTaxFormMode == FormMode.Edit)
                await HandleEditSubmit();
        }

        protected async Task EndAddEditAsync()
        {
            SalesTaxFormMode = FormMode.Unknown;
            SalesTaxes = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
            SelectedSalesTax = SalesTaxes.Where(x => x.Id == Id).FirstOrDefault();
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }
    }
}
