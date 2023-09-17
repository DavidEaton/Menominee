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

        public ManufacturersController(IManufacturerRepository repository, ILogger<ManufacturersController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<ManufacturerToReadInList>>> GetListAsync()
        {
            var result = await repository.GetManufacturerListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ManufacturerToRead>> GetAsync(long id)
        {
            var result = await repository.GetManufacturerAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(ManufacturerToWrite manufacturerToAdd)
        {
            var existingPrefixes = await repository.GetExistingPrefixList();
            var existingIds = await repository.GetExistingIdList();

            var result = Manufacturer.Create(
                repository.DetermineManufacturerId(existingIds), // we are not auto-incrementing the manufacturer id, user created manufacturers id starts at 50,000
                manufacturerToAdd.Name,
                manufacturerToAdd?.Prefix,
                existingPrefixes,
                existingIds);

            if (result.IsFailure)
                return BadRequest(result.Error);

            await repository.AddManufacturerAsync(result.Value);

            await repository.ExecuteInTransactionAsync(async () =>
            {
                await repository.ToggleIdentityInsert(true);
                await repository.SaveChangesAsync();
                await repository.ToggleIdentityInsert(false);
            });

            return CreatedAtAction(
                nameof(GetAsync),
                new { id = result.Value.Id },
                new { result.Value.Id });
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(long id, ManufacturerToWrite manufacturerFromCaller)
        {
            var existingPrefixes = await repository.GetExistingPrefixList();

            var manufacturerFromRepository = await repository.GetManufacturerEntityAsync(id);
            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer to update: {manufacturerFromCaller.Name}");

            var setNameResult = manufacturerFromRepository.SetName(manufacturerFromCaller.Name);
            if (setNameResult.IsFailure)
                return BadRequest(setNameResult.Error);

            var setPrefixResult = manufacturerFromRepository.SetPrefix(manufacturerFromCaller.Prefix, existingPrefixes);
            if (setPrefixResult.IsFailure)
                return BadRequest(setPrefixResult.Error);

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
