using Menominee.Client.Shared;
using Menominee.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders.Pages;

public partial class RepairOrderEditPage : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IRepairOrderDataService DataService { get; set; }

    [Parameter]
    public long Id { get; set; }

    [CascadingParameter(Name = "MainLayout")]
    MainLayout MainLayout { get; set; }

    [Inject]
    ILogger<RepairOrderEditPage> Logger { get; set; }

    private RepairOrderToWrite? repairOrderToEdit;

    public bool parametersSet { get; set; } = false;

    protected override async Task OnParametersSetAsync()
    {
        if (parametersSet)
            return;
        parametersSet = true;
        MainLayout?.ToggleRepairOrderEditMenuDisplay(true);

        var result = await DataService.GetAsync(Id);

        if (result.IsFailure)
        {
            Logger.LogError(result.Error);
            return;
        }

        repairOrderToEdit ??= Id == 0
            ? new()
            {
                DateCreated = DateTime.Today
            }
            : RepairOrderHelper.ConvertReadToWriteDto(result.Value);

        await base.OnParametersSetAsync();
    }

    private async Task Save()
    {
        if (Valid())
        {
            if (Id == 0)
            {
                //await DataService.AddRepairOrder(RepairOrderToEdit);
            }
            else
            {
                //await DataService.UpdateRepairOrder(RepairOrderToEdit, Id);
            }

            EndEdit();
        }
    }

    private bool Valid()
    {
        //if (Invoice.VendorId > 0 && Invoice.Date.HasValue)
        //    return true;

        //return false;
        return true;
    }

    private void Discard()
    {
        EndEdit();
    }

    protected void EndEdit()
    {
        MainLayout.ToggleRepairOrderEditMenuDisplay(false);
        NavigationManager.NavigateTo($"repairorders/worklog/{Id}");
    }
}
