using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages;

public partial class SalesTaxListPage : ComponentBase
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    [Inject]
    public ISalesTaxDataService? SalesTaxDataService { get; set; }

    [Parameter]
    public long Id { get; set; } = 0;

    private IReadOnlyList<SalesTaxToReadInList>? SalesTaxes;
    private IEnumerable<SalesTaxToReadInList> SelectedSalesTaxes { get; set; } = Enumerable.Empty<SalesTaxToReadInList>();
    private SalesTaxToReadInList? SelectedSalesTax { get; set; }
    private SalesTaxToWrite? SalesTax { get; set; } = null;
    private TelerikGrid<SalesTaxToReadInList>? Grid { get; set; }
    private bool CanEdit => Id > 0;
    private bool CanDelete => Id > 0;
    private FormMode SalesTaxFormMode = FormMode.Unknown;

    protected override async Task OnInitializedAsync()
    {
        await GetSalesTaxes();

        if (SalesTaxes?.Count > 0)
        {
            SelectedSalesTax = SalesTaxes.FirstOrDefault();
            if (SelectedSalesTax is not null)
            {
                Id = SelectedSalesTax.Id;
                SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
            }
        }
    }

    private void OnAdd()
    {
        SalesTaxFormMode = FormMode.Add;
        SalesTax = new()
        {
            IsAppliedByDefault = true,
            TaxType = SalesTaxType.Normal
        };
    }

    private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
    {
        Id = ((SalesTaxToReadInList)args.Item).Id;
        await OnEditAsync();
    }

    private async Task OnEditAsync()
    {
        if (Id > 0 && SalesTaxDataService is not null)
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
        if (SelectedSalesTax is not null)
        {
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }
    }

    private void OnRowSelected(GridRowClickEventArgs args)
    {
        Id = ((SalesTaxToReadInList)args.Item).Id;
    }

    private void OnDone() => NavigationManager?.NavigateTo("/settings/");

    protected async Task HandleAddSubmitAsync()
    {
        if (SalesTaxDataService is not null && SalesTax is not null)
        {
            Id = (await SalesTaxDataService.AddAsync(SalesTax)).Value.Id;
            await EndAddEditAsync();
            Grid?.Rebind();
        }
    }

    protected async Task HandleEditSubmitAsync()
    {
        if (SalesTaxDataService is not null && SalesTax is not null)
        {
            await SalesTaxDataService.UpdateAsync(SalesTax);
            await EndAddEditAsync();
        }
    }

    protected async Task SubmitHandlerAsync()
    {
        if (SalesTaxFormMode == FormMode.Add)
        {
            await HandleAddSubmitAsync();
        }
        else if (SalesTaxFormMode == FormMode.Edit)
        {
            await HandleEditSubmitAsync();
        }
    }

    protected async Task EndAddEditAsync()
    {
        SalesTaxFormMode = FormMode.Unknown;
        await GetSalesTaxes();

        SelectedSalesTax = SalesTaxes?.FirstOrDefault(x => x.Id == Id);
        if (SelectedSalesTax is not null)
        {
            SelectedSalesTaxes = new List<SalesTaxToReadInList> { SelectedSalesTax };
        }
    }

    private async Task GetSalesTaxes()
    {
        if (SalesTaxDataService is not null)
        {
            var result = await SalesTaxDataService.GetAllAsync();

            if (result.IsSuccess)
            {
                SalesTaxes = result.Value
                    .OrderBy(tax => tax.Order)
                    .ToList();
            }
        }
    }
}
