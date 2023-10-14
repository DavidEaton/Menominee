using Menominee.Client.Services.Customers;
using Menominee.Client.Services.Vehicles;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders;

public partial class RepairOrderCustomerVehicleTab
{
    [Inject]
    private IVehicleDataService VehicleDataService { get; set; }
    [Inject]
    private ICustomerDataService CustomerDataService { get; set; }

    [Parameter]
    public RepairOrderToWrite RepairOrderToEdit { get; set; }

    [Parameter]
    public EventCallback<RepairOrderToWrite> RepairOrderToEditChanged { get; set; }

    private RepairOrderToWrite BoundRepairOrderToEdit
    {
        get => RepairOrderToEdit;
        set => RepairOrderToEditChanged.InvokeAsync(value);
    }

    private FormMode EditVehicleFormMode = FormMode.Unknown;
    private VehicleLookupMode VehicleLookupMode = VehicleLookupMode.Unknown;
    private FormMode EditCustomerFormMode = FormMode.Unknown;
    private CustomerLookupMode CustomerLookupMode = CustomerLookupMode.Unknown;

    private void AddCustomer()
    {
        EditCustomerFormMode = FormMode.Add;
    }

    private void EditCustomer()
    {
        EditCustomerFormMode = FormMode.Edit;
    }

    private async Task SaveCustomer(CustomerToWrite customer)
    {
        if (customer.Id > 0)
        {
            var updateResult = await CustomerDataService.UpdateAsync(customer);

            if (updateResult.IsFailure)
            {
                // TODO: Alert user of failure
                return;
            }
        }
        else
        {
            var addResult = await CustomerDataService.AddAsync(customer);

            if (addResult.IsFailure)
            {
                // TODO: Alert user of failure
                return;
            }

            var saveResult = await CustomerDataService.GetAsync(addResult.Value.Id);

            if (saveResult.IsFailure)
            {
                // TODO: Alert user of failure
                return;
            }

            RepairOrderToEdit.Customer = saveResult.Value;
            customer.Id = saveResult.Value.Id;
        }

        await RepairOrderToEditChanged.InvokeAsync(RepairOrderToEdit);
        EditCustomerFormMode = FormMode.Unknown;
    }

    private void LookupCustomer(CustomerLookupMode lookup) => CustomerLookupMode = lookup;

    private void DiscardCustomer()
    {
        EditCustomerFormMode = FormMode.Unknown;
    }

    private void AddVehicle()
    {
        EditVehicleFormMode = FormMode.Add;
    }

    private void EditVehicle()
    {
        EditVehicleFormMode = FormMode.Edit;
    }

    private async Task SaveVehicle(VehicleToWrite vehicle)
    {
        if (vehicle.Id > 0)
        {
            var result = await VehicleDataService.UpdateAsync(vehicle);

            if (result.IsFailure)
            {
                return;
            }
        }
        else
        {
            var result = await VehicleDataService.AddAsync(vehicle);

            if (result.IsFailure)
            {
                return;
            }

            vehicle.Id = result.Value;
        }

        RepairOrderToEdit.Vehicle = VehicleHelper.ConvertWriteToReadDto(vehicle);
        //RepairOrderToEdit.Vehicle = new()
        //{
        //    Id = vehicle.Id,
        //    VIN = vehicle.VIN,
        //    Year = vehicle.Year,
        //    Make = vehicle.Make,
        //    Model = vehicle.Model,
        //    Plate = vehicle.Plate,
        //    PlateStateProvince = vehicle.PlateStateProvince,
        //    UnitNumber = vehicle.UnitNumber,
        //    Color = vehicle.Color,
        //    Active = vehicle.Active,
        //    NonTraditionalVehicle = vehicle.NonTraditionalVehicle
        //};

        await RepairOrderToEditChanged.InvokeAsync(RepairOrderToEdit);
        EditVehicleFormMode = FormMode.Unknown;
    }

    private void LookupVehicle(VehicleLookupMode lookup) => VehicleLookupMode = lookup;

    private async Task SelectVehicle(VehicleToRead vehicle)
    {
        RepairOrderToEdit.Vehicle = vehicle;
        await RepairOrderToEditChanged.InvokeAsync(RepairOrderToEdit);
        VehicleLookupMode = VehicleLookupMode.Unknown;
    }

    private void CancelVehicleLookup()
    {
        VehicleLookupMode = VehicleLookupMode.Unknown;
    }

    private void DiscardVehicle()
    {
        EditVehicleFormMode = FormMode.Unknown;
    }
}
