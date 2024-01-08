using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Vehicles;

public class VehiclesController : BaseApplicationController<VehiclesController>
{
    private readonly IVehicleRepository repository;

    public VehiclesController(
        IVehicleRepository repository,
        ILogger<VehiclesController> logger) : base(logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("list/{customerId:long}")]
    public async Task<ActionResult<IReadOnlyList<VehicleToRead>>> GetAsync(
        long customerId,
        [FromQuery] SortOrder sortOrder = SortOrder.Asc,
        VehicleSortColumn sortColumn = VehicleSortColumn.Plate,
        bool includeInactive = false,
        string searchTerm = "")
    {
        var vehicles = await repository
            .GetEntitiesAsync(customerId, sortOrder, sortColumn, includeInactive, searchTerm);

        return vehicles is null
            ? Ok(new List<VehicleToRead>())
            : Ok(vehicles
                .Select(vehicle => VehicleHelper.ConvertToReadDto(vehicle))
                .ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleToRead>> GetAsync(long id)
    {
        var vehicle = await repository.GetEntityAsync(id);

        return vehicle is not null
            ? Ok(VehicleHelper.ConvertToReadDto(vehicle))
            : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> AddAsync(VehicleToWrite vehicleToAdd)
    {
        var vehicle = Vehicle.Create(
                    vehicleToAdd.VIN,
                    vehicleToAdd.Year,
                    vehicleToAdd.Make,
                    vehicleToAdd.Model,
                    vehicleToAdd.Plate,
                    vehicleToAdd.PlateStateProvince,
                    vehicleToAdd.UnitNumber,
                    vehicleToAdd.Color,
                    vehicleToAdd.Active).Value;

        repository.Add(vehicle);
        await repository.SaveChangesAsync();

        return Created(
            new Uri($"api/vehicles/{vehicle.Id}", UriKind.Relative),
            new { vehicle.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateAsync(VehicleToWrite vehicleToUpdate)
    {
        var vehicleFromRepository = await repository.GetEntityAsync(vehicleToUpdate.Id);
        if (vehicleFromRepository is null)
            return NotFound($"Vehicle with id {vehicleToUpdate.Id} not found.");

        if (VehiclesAreEqual(vehicleFromRepository, vehicleToUpdate))
            return NoContent();

        UpdateVehicle(vehicleToUpdate, vehicleFromRepository);

        await repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        var vehicleFromRepository = await repository.GetEntityAsync(id);
        if (vehicleFromRepository is null)
            return NotFound($"Vehicle with id {id} not found.");

        repository.Delete(vehicleFromRepository);
        await repository.SaveChangesAsync();

        return NoContent();
    }

    private static bool VehiclesAreEqual(Vehicle vehicleFromRepository, VehicleToWrite vehicleToUpdate) =>
        vehicleFromRepository.VIN == vehicleToUpdate.VIN
        && vehicleFromRepository.Year == vehicleToUpdate.Year
        && vehicleFromRepository.Make == vehicleToUpdate.Make
        && vehicleFromRepository.Model == vehicleToUpdate.Model
        && vehicleFromRepository.Plate == vehicleToUpdate.Plate
        && vehicleFromRepository.PlateStateProvince == vehicleToUpdate.PlateStateProvince
        && vehicleFromRepository.UnitNumber == vehicleToUpdate.UnitNumber
        && vehicleFromRepository.Color == vehicleToUpdate.Color
        && vehicleFromRepository.Active == vehicleToUpdate.Active;

    private static void UpdateVehicle(VehicleToWrite vehicleToUpdate, Vehicle vehicleFromRepository)
    {
        vehicleFromRepository.SetVin(vehicleToUpdate.VIN);
        vehicleFromRepository.SetYear(vehicleToUpdate.Year);
        vehicleFromRepository.SetMake(vehicleToUpdate.Make);
        vehicleFromRepository.SetModel(vehicleToUpdate.Model);
        vehicleFromRepository.SetPlate(vehicleToUpdate.Plate);
        vehicleFromRepository.SetPlateStateProvince(vehicleToUpdate.PlateStateProvince);
        vehicleFromRepository.SetUnitNumber(vehicleToUpdate.UnitNumber);
        vehicleFromRepository.SetColor(vehicleToUpdate.Color);
        vehicleFromRepository.SetActive(vehicleToUpdate.Active);
    }
}