using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.SaleCodes
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleCodesController : ControllerBase
    {
        private readonly ISaleCodeRepository repository;
        private readonly string BasePath = "/api/salecodes";

        public SaleCodesController(ISaleCodeRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/salecodes/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SaleCodeToReadInList>>> GetSaleCodeListAsync()
        {
            var results = await repository.GetSaleCodeListAsync();
            return Ok(results);
        }

        // api/salecodes/a
        [HttpGet("{code:string}")]
        public async Task<ActionResult<SaleCodeToRead>> GetSaleCodeAsync(string code)
        {
            var result = await repository.GetSaleCodeAsync(code);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/salecodes/a
        [HttpPut("{code:string}")]
        public async Task<IActionResult> UpdateSaleCodeAsync(string code, SaleCodeToWrite scDto)
        {
            var notFoundMessage = $"Could not find Sale Code to update: {scDto.Name}";

            if (!await repository.SaleCodeExistsAsync(code))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var sc = repository.GetSaleCodeEntityAsync(code).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            sc.Code = scDto.Code;
            sc.Name = scDto.Name;

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            sc.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateSaleCodeAsync(sc);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<SaleCodeToRead>> AddSaleCodeAsync(SaleCodeToWrite scCreateDto)
        {
            // 1. Convert dto to domain entity
            var sc = new SaleCode()
            {
                Code = scCreateDto.Code,
                Name = scCreateDto.Name
            };

            // 2. Add domain entity to repository
            await repository.AddSaleCodeAsync(sc);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return Created(new Uri($"{BasePath}/{sc.Code}", UriKind.Relative), new { Code = sc.Code });
        }

        [HttpDelete("{code:string}")]
        public async Task<IActionResult> DeleteSaleCodeAsync(string code)
        {
            var scFromRepository = await repository.GetSaleCodeAsync(code);
            if (scFromRepository == null)
                return NotFound($"Could not find Sale Code in the database to delete with Id: {code}.");

            await repository.DeleteSaleCodeAsync(code);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
