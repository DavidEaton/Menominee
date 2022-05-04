using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExciseFeesController : ControllerBase
    {
        private readonly IExciseFeeRepository repository;
        private readonly string BasePath = "/api/excisefees";
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
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ExciseFeeToRead>> GetExciseFeeAsync(long id)
        {
            var result = await repository.GetExciseFeeAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/excisefees/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateExciseFeeAsync(long id, ExciseFeeToWrite feeDto)
        {
            if (!await repository.ExciseFeeExistsAsync(id))
                return NotFound($"Could not find Excise Fee to update: {feeDto.Description}");

            //1) Get domain entity from repository
            var ef = repository.GetExciseFeeEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            ef.Description = feeDto.Description;
            ef.FeeType = feeDto.FeeType;
            ef.Amount = feeDto.Amount;

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            ef.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            await repository.UpdateExciseFeeAsync(ef);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddExciseFeeAsync(ExciseFeeToWrite efDto)
        {
            // 1. Convert dto to domain entity
            var ef = new ExciseFee()
            {
                Description = efDto.Description,
                FeeType = efDto.FeeType,
                Amount = efDto.Amount
            };

            // 2. Add domain entity to repository
            await repository.AddExciseFeeAsync(ef);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return Created(new Uri($"{BasePath}/{ef.Id}", UriKind.Relative), new { ef.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteExciseFeeAsync(long id)
        {
            // TODO - Is this where we should this delete the entries in the SalesTaxTaxableExciseFee table too?

            var efFromRepository = await repository.GetExciseFeeAsync(id);
            if (efFromRepository == null)
                return NotFound($"Could not find Excise Fee in the database to delete with Id: {id}.");

            await repository.DeleteExciseFeeAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
