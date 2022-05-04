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
    public class SalesTaxesController : ControllerBase
    {
        private readonly ISalesTaxRepository repository;
        private readonly string BasePath = "/api/salestaxes";
        public SalesTaxesController(ISalesTaxRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/salestaxes/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SalesTaxToReadInList>>> GetSalesTaxListAsync()
        {
            var results = await repository.GetSalesTaxListAsync();
            return Ok(results);
        }

        // api/salestaxes/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<SalesTaxToRead>> GetSalesTaxAsync(long id)
        {
            var result = await repository.GetSalesTaxAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/salestaxes/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateSalesTaxAsync(long id, SalesTaxToWrite taxToWrite)
        {
            if (!await repository.SalesTaxExistsAsync(id))
                return NotFound($"Could not find Sales Tax to update: {taxToWrite.Description}");

            //1) Get domain entity from repository
            var tax = repository.GetSalesTaxEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            tax.Description = taxToWrite.Description;
            tax.TaxType = taxToWrite.TaxType;
            tax.Order = taxToWrite.Order;
            tax.IsAppliedByDefault = taxToWrite.IsAppliedByDefault;
            tax.IsTaxable = taxToWrite.IsTaxable;
            tax.TaxIdNumber = taxToWrite.TaxIdNumber;
            tax.PartTaxRate = taxToWrite.PartTaxRate;
            tax.LaborTaxRate = taxToWrite.LaborTaxRate;
            // TODO - also add TaxedExciseFees

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            tax.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            await repository.UpdateSalesTaxAsync(tax);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddSalesTaxAsync(SalesTaxToWrite taxToWrite)
        {
            // 1. Convert dto to domain entity
            var tax = new SalesTax()
            {
                Description = taxToWrite.Description,
                TaxType = taxToWrite.TaxType,
                Order = taxToWrite.Order,
                IsAppliedByDefault = taxToWrite.IsAppliedByDefault,
                IsTaxable = taxToWrite.IsTaxable,
                TaxIdNumber = taxToWrite.TaxIdNumber,
                PartTaxRate = taxToWrite.PartTaxRate,
                LaborTaxRate = taxToWrite.LaborTaxRate
                // TODO - deal with TaxedExciseFees
            };

            // 2. Add domain entity to repository
            await repository.AddSalesTaxAsync(tax);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return Created(new Uri($"{BasePath}/{tax.Id}", UriKind.Relative), new { tax.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteSalesTaxAsync(long id)
        {
            // TODO - Is this where we should this delete the entries in the SalesTaxTaxableExciseFee table too?

            var tax = await repository.GetSalesTaxAsync(id);
            if (tax == null)
                return NotFound($"Could not find Sales Tax in the database to delete with Id: {id}.");

            await repository.DeleteSalesTaxAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
