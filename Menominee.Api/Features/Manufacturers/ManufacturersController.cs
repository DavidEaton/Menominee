using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Manufacturers
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
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ManufacturerToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(ManufacturerToWrite manufacturerToAdd)
        {
            var existingPrefixes = await repository.GetExistingPrefixListAsync();
            var existingIds = await repository.GetExistingIdsAsync();

            // No need to validate it here again, just call .Value right away
            var manufacturer = Manufacturer.Create(
                repository.GetNextManufacturerId(existingIds), // we are not auto-incrementing the manufacturer id, user created manufacturers id starts at 50,000
                manufacturerToAdd.Name,
                manufacturerToAdd?.Prefix,
                existingPrefixes,
                existingIds).Value;

            repository.Add(manufacturer);

            await repository.ExecuteInTransactionAsync(async () =>
            {
                await repository.ToggleIdentityInsertAsync(true);
                await repository.SaveChangesAsync();
                await repository.ToggleIdentityInsertAsync(false);
            });

            return Created(new Uri($"api/creditCardscontroller/{manufacturer.Id}",
                UriKind.Relative),
                new { manufacturer.Id });
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(ManufacturerToWrite manufacturerFromCaller)
        {
            var existingPrefixes = await repository.GetExistingPrefixListAsync();

            var manufacturerFromRepository = await repository.GetEntityAsync(manufacturerFromCaller.Id);
            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer to update: {manufacturerFromCaller.Name}");

            manufacturerFromRepository.SetName(manufacturerFromCaller.Name);
            manufacturerFromRepository.SetPrefix(manufacturerFromCaller.Prefix, existingPrefixes);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteManufacturerAsync(long id)
        {
            var manufacturerFromRepository = await repository.GetEntityAsync(id);

            if (manufacturerFromRepository is null)
                return NotFound($"Could not find Manufacturer in the database to delete with Id: {id}.");

            repository.Delete(manufacturerFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
