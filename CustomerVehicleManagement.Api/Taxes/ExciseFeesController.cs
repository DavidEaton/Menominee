using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    public class ExciseFeesController : ApplicationController
    {
        private readonly IExciseFeeRepository repository;

        public ExciseFeesController(IExciseFeeRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/excisefees/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ExciseFeeToReadInList>>> GetExciseFeeListAsync()
        {
            var results = await repository.GetExciseFeeListAsync();
            return Ok(results);
        }

        // api/excisefees/1
        [HttpGet("{id:long}", Name = "GetExciseFeeAsync")]
        public async Task<ActionResult<ExciseFeeToRead>> GetExciseFeeAsync(long id)
        {
            var result = await repository.GetExciseFeeAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/excisefees/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateExciseFeeAsync(long id, ExciseFeeToUpdate exciseFee)
        {
            if (!await repository.ExciseFeeExistsAsync(id))
                return NotFound($"Could not find Excise Fee to update: {exciseFee.Description}");

            //1) Get domain entity from repository
            var exciseFeeFromRepository = await repository.GetExciseFeeEntityAsync(id);

            // 2) Update domain entity with data in data transfer object(DTO)

            exciseFeeFromRepository.SetDescription(exciseFee.Description);
            exciseFeeFromRepository.SetFeeType(exciseFee.FeeType);
            exciseFeeFromRepository.SetAmount(exciseFee.Amount);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ExciseFeeToRead>> AddExciseFeeAsync(ExciseFeeToAdd exciseFeeToAdd)
        {
            // 1. Convert dto to domain entity
            var exciseFee = ExciseFeeHelper.ConvertAddDtoToEntity(exciseFeeToAdd);

            // 2. Add domain entity to repository
            await repository.AddExciseFeeAsync(exciseFee);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new fee from database to consumer after save
            return CreatedAtRoute("GetExciseFeeAsync",
                new { exciseFee.Id },
                ExciseFeeHelper.ConvertEntityToReadDto(exciseFee));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteExciseFeeAsync(long id)
        {
            // TODO - Is this where we should this delete the entries in the SalesTaxTaxableExciseFee table too?
            // Yes if cascade deelets ==  true
            var exciseFeeFromRepository = await repository.GetExciseFeeEntityAsync(id);

            if (exciseFeeFromRepository is null)
                return NotFound($"Could not find Excise Fee in the database to delete with Id: {id}.");

            repository.DeleteExciseFee(exciseFeeFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
