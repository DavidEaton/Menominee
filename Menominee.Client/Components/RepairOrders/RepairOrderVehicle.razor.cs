﻿using Menominee.Domain.Enums;
using Menominee.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders;

public partial class RepairOrderVehicle
{
    [Parameter]
    public RepairOrderToWrite RepairOrderToEdit { get; set; }

    [Parameter]
    public EventCallback<RepairOrderToWrite> RepairOrderToEditChanged { get; set; }

    [Parameter]
    public EventCallback OnAdd { get; set; }

    [Parameter]
    public EventCallback OnEdit { get; set; }

    [Parameter]
    public EventCallback<VehicleLookupMode> OnLookup { get; set; }

    private bool CanEdit => RepairOrderToEdit.Vehicle is not null && RepairOrderToEdit.Vehicle.Id > 0;

    private async Task AddVehicle()
    {
        await OnAdd.InvokeAsync();
    }

    private async Task EditVehicle()
    {
        await OnEdit.InvokeAsync();
    }

    private async Task LookupVehicle(VehicleLookupMode lookupMode)
    {
        await OnLookup.InvokeAsync(lookupMode);
    }

    private void ViewNotes()
    {
    }
}
