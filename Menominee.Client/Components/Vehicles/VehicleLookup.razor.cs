using Menominee.Client.Services.Vehicles;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Vehicles;

public partial class VehicleLookup
{
    [Inject]
    private IVehicleDataService VehicleDataService { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public long CustomerId { get; set; }

    [Parameter]
    public VehicleLookupMode LookupMode { get; set; }

    [Parameter]
    public EventCallback<VehicleToRead> OnSelect { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private string Title => $"Vehicle Lookup";
    private VehicleLookupModel LookupModel { get; set; } = new();
    private IReadOnlyList<VehicleToRead> GridData { get; set; } = new List<VehicleToRead>();
    private IEnumerable<VehicleToRead> SelectedItems { get; set; } = Enumerable.Empty<VehicleToRead>();
    private List<VehicleSortColumn> sortColumns = new() { VehicleSortColumn.Plate, VehicleSortColumn.UnitNumber, VehicleSortColumn.VIN };

    CancellationTokenSource tokenSource = new CancellationTokenSource();

    protected override async Task OnInitializedAsync()
    {
        LookupModel.SortColumn = Enum.TryParse<VehicleSortColumn>(LookupMode.ToString(), out var column) ? column : LookupModel.SortColumn;

        await SearchVehicles();

        await base.OnInitializedAsync();
    }

    private async Task HandleSortColumnChange(VehicleSortColumn sortColumn)
    {
        LookupModel.SortColumn = sortColumn;

        if (LookupModel.SearchTerm.Length > 0)
        {
            await SearchVehicles();
        }
    }

    private async Task HandleSearchChange(string searchInput)
    {
        LookupModel.SearchTerm = searchInput;

        await Debounce(500);
        await SearchVehicles();
    }

    private async Task Debounce(int millisecondsDelay)
    {
        tokenSource.Cancel();
        tokenSource.Dispose();

        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        await Task.Delay(millisecondsDelay, token);
    }

    private async Task HandleRowDoubleClick(GridRowClickEventArgs args)
    {
        var vehicle = args.Item as VehicleToRead;
        await OnSelect.InvokeAsync(vehicle);
    }

    private async Task HandleIncludeInactiveChange(bool includeInactive)
    {
        LookupModel.IncludeInactive = includeInactive;
        await SearchVehicles();
    }

    private async Task HandleSelect()
    {
        var vehicle = SelectedItems.FirstOrDefault();

        if (vehicle is null) return;

        await OnSelect.InvokeAsync(vehicle);
    }

    private async Task HandleCancel()
    {
        await OnCancel.InvokeAsync();
    }

    private async Task SearchVehicles()
    {
        var result = await VehicleDataService.GetByParametersAsync(CustomerId, LookupModel.SortOrder, LookupModel.SortColumn, LookupModel.IncludeInactive, LookupModel.SearchTerm);

        if (result.IsFailure) return;

        GridData = result.Value;
    }

    private class VehicleLookupModel
    {
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
        public VehicleSortColumn SortColumn { get; set; } = VehicleSortColumn.Plate;
        public bool IncludeInactive { get; set; } = false;
        public string SearchTerm { get; set; } = string.Empty;
    }
}
