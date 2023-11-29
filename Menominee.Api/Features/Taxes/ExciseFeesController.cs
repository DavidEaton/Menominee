using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Taxes
{
    public class ExciseFeesController : BaseApplicationController<ExciseFeesController>
    {
        private readonly IExciseFeeRepository repository;

        public ExciseFeesController(IExciseFeeRepository repository, ILogger<ExciseFeesController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<ExciseFeeToReadInList>>> GetListAsync()
        {
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ExciseFeeToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(ExciseFeeToWrite exciseFeeToUpdate)
        {
            var exciseFeeFromRepository = await repository.GetEntityAsync(exciseFeeToUpdate.Id);
            if (exciseFeeFromRepository is null)
                return NotFound($"Could not find Excise Fee to update: {exciseFeeToUpdate.Description}");

            if (ExciseFeesAreEqual(exciseFeeFromRepository, exciseFeeToUpdate))
                return NoContent();

            exciseFeeFromRepository.SetDescription(exciseFeeToUpdate.Description);
            exciseFeeFromRepository.SetFeeType(exciseFeeToUpdate.FeeType);
            exciseFeeFromRepository.SetAmount(exciseFeeToUpdate.Amount);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ExciseFeeToRead>> AddAsync(ExciseFeeToAdd exciseFeeToAdd)
        {
            var exciseFee = ExciseFee.Create(
                exciseFeeToAdd.Description,
                exciseFeeToAdd.FeeType,
                exciseFeeToAdd.Amount)
                .Value;

            repository.Add(exciseFee);
            await repository.SaveChangesAsync();

            return Created(
                new Uri($"api/ExciseFeesController/{exciseFee.Id}", UriKind.Relative),
                new { exciseFee.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            // TODO - Is this where we should this delete the entries in the SalesTaxTaxableExciseFee table too?
            // Yes if cascade deelets ==  true
            var exciseFeeFromRepository = await repository.GetEntityAsync(id);
            if (exciseFeeFromRepository is null)
                return NotFound($"Could not find Excise Fee in the database to delete with Id: {id}.");

            repository.Delete(exciseFeeFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static bool ExciseFeesAreEqual(ExciseFee exciseFeeFromRepository, ExciseFeeToWrite exciseFeeToUpdate) =>
            exciseFeeFromRepository.Description == exciseFeeToUpdate.Description
                && exciseFeeFromRepository.FeeType == exciseFeeToUpdate.FeeType
                && exciseFeeFromRepository.Amount == exciseFeeToUpdate.Amount;
    }
}
