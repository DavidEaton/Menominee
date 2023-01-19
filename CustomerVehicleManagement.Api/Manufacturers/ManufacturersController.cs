using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Manufacturers
{
    public class ManufacturersController : ApplicationController
    {
        private readonly IManufacturerRepository repository;
        private readonly string BasePath = "/api/manufacturers";

        public ManufacturersController(IManufacturerRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<ManufacturerToReadInList>>> GetManufacturerListAsync()
        {
            var result = await repository.GetManufacturerListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<ManufacturerToRead>> GetManufacturerAsync(string code)
        {
            var result = await repository.GetManufacturerAsync(code);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ManufacturerToRead>> GetManufacturerAsync(long id)
        {
            var result = await repository.GetManufacturerAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateManufacturerAsync(long id, ManufacturerToWrite manufacturerFromCaller)
        {
            //1) Get domain entity from repository
            var manufacturerFromRepository = await repository.GetManufacturerEntityAsync(id);

            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer to update: {manufacturerFromCaller.Name}");

            // 2) Update domain entity with data in data transfer object(DTO)
            manufacturerFromRepository.SetCode(manufacturerFromCaller.Code);
            manufacturerFromRepository.SetName(manufacturerFromCaller.Name);
            manufacturerFromRepository.SetPrefix(manufacturerFromCaller.Prefix);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddManufacturerAsync(ManufacturerToWrite manufacturerToAdd)
        {
            // 1. Convert dto to domain entity
            var resultOrError = Manufacturer.Create(
                manufacturerToAdd.Name,
                manufacturerToAdd.Prefix,
                manufacturerToAdd.Code);

            if (resultOrError.IsFailure)
                return BadRequest(resultOrError.Error);

            // 2. Add domain entity to repository
            await repository.AddManufacturerAsync(resultOrError.Value);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return to caller
            return Created(
                new Uri($"{BasePath}/{resultOrError.Value.Id}",
                UriKind.Relative),
                new
                {
                    resultOrError.Value.Id
                });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteManufacturerAsync(long id)
        {
            var manufacturerFromRepository = await repository.GetManufacturerAsync(id);

            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer in the database to delete with Id: {id}.");

            await repository.DeleteManufacturerAsync(id);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
