using Menominee.Api.Common;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Manufacturers
{
    public class ManufacturersController : BaseApplicationController<ManufacturersController>
    {
        private readonly IManufacturerRepository repository;
        private readonly string BasePath = "/api/manufacturers";

        public ManufacturersController(IManufacturerRepository repository, ILogger<ManufacturersController> logger) : base(logger)
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

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ManufacturerToRead>> GetManufacturerAsync(long id)
        {
            var result = await repository.GetManufacturerAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddManufacturerAsync(ManufacturerToWrite manufacturerToAdd)
        {
            var existingPrefixes = await repository.GetExistingPrefixList();
            var existingIds = await repository.GetExistingIdList();

            // 1. Convert dto to domain entity
            var resultOrError = Manufacturer.Create(
                repository.DetermineManufacturerId(existingIds), // we are not auto-incrementing the manufacturer id, user created manufacturers id starts at 50,000
                manufacturerToAdd.Name,
                manufacturerToAdd?.Prefix,
                existingPrefixes,
                existingIds);

            if (resultOrError.IsFailure)
                return BadRequest(resultOrError.Error);

            // 2. Add domain entity to repository
            await repository.AddManufacturerAsync(resultOrError.Value);

            // 3. Save changes on repository
            await repository.ExecuteInTransactionAsync(async () =>
            {
                await repository.ToggleIdentityInsert(true);
                await repository.SaveChangesAsync();
                await repository.ToggleIdentityInsert(false);
            });

            // 4. Return to caller
            return Created(
                new Uri($"{BasePath}/{resultOrError.Value.Id}",
                UriKind.Relative),
                new
                {
                    resultOrError.Value.Id
                });
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateManufacturerAsync(long id, ManufacturerToWrite manufacturerFromCaller)
        {
            var existingPrefixes = await repository.GetExistingPrefixList();
            //1) Get domain entity from repository
            var manufacturerFromRepository = await repository.GetManufacturerEntityAsync(id);

            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer to update: {manufacturerFromCaller.Name}");

            // 2) Update domain entity with data in data transfer object(DTO)
            manufacturerFromRepository.SetName(manufacturerFromCaller.Name);
            manufacturerFromRepository.SetPrefix(manufacturerFromCaller.Prefix, existingPrefixes);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteManufacturerAsync(long id)
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
