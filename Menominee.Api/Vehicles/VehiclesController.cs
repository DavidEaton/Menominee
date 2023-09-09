using Menominee.Api.Common;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Vehicles;

public class VehiclesController : BaseApplicationController<VehiclesController>
{
    private readonly IVehicleRepository repository;
    private readonly string BasePath = "/api/vehicles";

    public VehiclesController(
        IVehicleRepository repository,
        ILogger<VehiclesController> logger) : base(logger)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("list/{customerId:long}")]
    public async Task<ActionResult<IReadOnlyList<VehicleToRead>>> GetVehiclesAsync(long customerId, [FromQuery] SortOrder sortOrder = SortOrder.Asc, VehicleSortColumn sortColumn = VehicleSortColumn.Plate, bool includeInactive = false, string searchTerm = "")
    {
        var vehicles = await repository.GetVehiclesAsync(customerId, sortOrder, sortColumn, includeInactive, searchTerm);

        return vehicles is null
            ? Ok(new List<VehicleToRead>())
            : Ok(vehicles
                .Select(vehicle => VehicleHelper.ConvertToReadDto(vehicle))
                .ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleToRead>> GetVehicleAsync(long id)
    {
        var vehicle = await repository.GetEntityAsync(id);

        return vehicle is not null
            ? Ok(VehicleHelper.ConvertToReadDto(vehicle))
            : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> AddVehicleAsync([FromBody] VehicleToWrite vehicleToAdd)
    {
        var vehicle = VehicleHelper.ConvertWriteDtoToEntity(vehicleToAdd);

        var vehicleFromRepository = await repository.GetEntityAsync(vehicleToAdd.VIN);
        if (vehicleFromRepository is not null)
        {
            return Conflict();
        }

        repository.AddVehicle(vehicle);
        await repository.SaveChanges();

        return Created(
            new Uri($"{BasePath}/{vehicle.Id}", UriKind.Relative),
            new { vehicle.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateVehicleAsync(long id, [FromBody] VehicleToWrite vehicleToUpdate)
    {
        var notFoundMessage = $"Vehicle with id {id} not found.";

        var vehicleFromRepository = await repository.GetEntityAsync(id);
        if (vehicleFromRepository is null)
        {
            return NotFound(notFoundMessage);
        }

        var vinResult = vehicleFromRepository.SetVin(vehicleToUpdate.VIN);
        if (vinResult.IsFailure)
        {
            return BadRequest(vinResult.Error);
        }

        var yearResult = vehicleFromRepository.SetYear(vehicleToUpdate.Year);
        if (yearResult.IsFailure)
        {
            return BadRequest(yearResult.Error);
        }

        var makeResult = vehicleFromRepository.SetMake(vehicleToUpdate.Make);
        if (makeResult.IsFailure)
        {
            return BadRequest(makeResult.Error);
        }

        var modelResult = vehicleFromRepository.SetModel(vehicleToUpdate.Model);
        if (modelResult.IsFailure)
        {
            return BadRequest(modelResult.Error);
        }

        var plateResult = vehicleFromRepository.SetPlate(vehicleToUpdate.Plate);
        if (plateResult.IsFailure)
        {
            return BadRequest(plateResult.Error);
        }

        var plateStateProvinceResult = vehicleFromRepository.SetPlateStateProvince(vehicleToUpdate.PlateStateProvince);
        if (plateStateProvinceResult.IsFailure)
        {
            return BadRequest(plateStateProvinceResult.Error);
        }

        var unitNumberResult = vehicleFromRepository.SetUnitNumber(vehicleToUpdate.UnitNumber);
        if (unitNumberResult.IsFailure)
        {
            return BadRequest(unitNumberResult.Error);
        }

        var colorResult = vehicleFromRepository.SetColor(vehicleToUpdate.Color);
        if (colorResult.IsFailure)
        {
            return BadRequest(colorResult.Error);
        }

        vehicleFromRepository.SetActive(vehicleToUpdate.Active);

        await repository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteVehicleAsync(long id)
    {
        var notFoundMessage = $"Vehicle with id {id} not found.";

        var vehicleFromRepository = await repository.GetEntityAsync(id);
        if (vehicleFromRepository is null)
        {
            return NotFound(notFoundMessage);
        }

        repository.DeleteVehicle(vehicleFromRepository);
        await repository.SaveChanges();

        return NoContent();
    }
}