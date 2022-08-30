using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Manufacturers
{
    public class ManufacturersController : ApplicationController
    {
        private readonly IManufacturerRepository repository;
        //private readonly string BasePath = "/api/manufacturers";

        public ManufacturersController(IManufacturerRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/manufacturers/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ManufacturerToReadInList>>> GetManufacturerListAsync()
        {
            var results = await repository.GetManufacturerListAsync();
            return Ok(results);
        }

        // api/manufacturers
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<ManufacturerToRead>>> GetManufacturersAsync()
        //{
        //    var result = await repository.GetManufacturersAsync();
        //    return Ok(result);
        //}

        // api/manufacturers/xyz
        [HttpGet("code/{code}")]
        public async Task<ActionResult<ManufacturerToRead>> GetManufacturerAsync(string code)
        {
            var result = await repository.GetManufacturerAsync(code);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/manufacturers/1
        [HttpGet("{id:long}", Name = "GetManufacturerAsync")]
        public async Task<ActionResult<ManufacturerToRead>> GetManufacturerAsync(long id)
        {
            var result = await repository.GetManufacturerAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/manufacturers/xyz
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateManufacturerAsync(string code, ManufacturerToWrite mfrDto)
        {
            var notFoundMessage = $"Could not find Manufacturer to update: {mfrDto.Name}";

            if (!await repository.ManufacturerExistsAsync(code))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var manufacturer = repository.GetManufacturerEntityAsync(code).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            manufacturer.SetCode(mfrDto.Code);
            manufacturer.SetName(mfrDto.Name);
            manufacturer.SetPrefix(mfrDto.Prefix);

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            manufacturer.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateManufacturerAsync(manufacturer);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ManufacturerToRead>> AddManufacturerAsync(ManufacturerToWrite mfrCreateDto)
        {
            // 1. Convert dto to domain entity
            var manufacturer = Manufacturer.Create(
                mfrCreateDto.Name,
                mfrCreateDto.Prefix, 
                mfrCreateDto.Code).Value;

            // 2. Add domain entity to repository
            await repository.AddManufacturerAsync(manufacturer);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            //return Created(new Uri($"{BasePath}/{mfr.Code}", UriKind.Relative), new { mfr.Code });
            return CreatedAtRoute("GetManufacturerAsync",
                                  new { id = manufacturer.Id },
                                  ManufacturerHelper.ConvertEntityToReadDto(manufacturer));
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteManufacturerAsync(string code)
        {
            var mfrFromRepository = await repository.GetManufacturerAsync(code);
            if (mfrFromRepository == null)
                return NotFound($"Could not find Manufacturer in the database to delete with Id: {code}.");

            await repository.DeleteManufacturerAsync(code);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
