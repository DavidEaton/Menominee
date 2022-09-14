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
    public class SalesTaxesController : ApplicationController
    {
        private readonly ISalesTaxRepository repository;
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
        [HttpGet("{id:long}", Name = "GetSalesTaxAsync")]
        public async Task<ActionResult<SalesTaxToRead>> GetSalesTaxAsync(long id)
        {
            var result = await repository.GetSalesTaxAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/salestaxes/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateSalesTaxAsync(long id, SalesTaxToWrite salesTax)
        {
            if (!await repository.SalesTaxExistsAsync(id))
                return NotFound($"Could not find Sales Tax to update: {salesTax.Description}");

            //1) Get domain entity from repository
            SalesTax taxFromRepository = await repository.GetSalesTaxEntityAsync(id);

            // 2) Update domain entity with data in data transfer object(DTO)
            taxFromRepository.SetDescription(salesTax.Description);
            taxFromRepository.SetTaxType(salesTax.TaxType);
            taxFromRepository.SetOrder(salesTax.Order);
            taxFromRepository.SetIsAppliedByDefault(salesTax.IsAppliedByDefault);
            taxFromRepository.SetIsTaxable(salesTax.IsTaxable);
            taxFromRepository.SetTaxIdNumber(salesTax.TaxIdNumber);
            taxFromRepository.SetPartTaxRate(salesTax.PartTaxRate);
            taxFromRepository.SetLaborTaxRate(salesTax.LaborTaxRate);
            //taxFromRepository.SetExciseFees(ExciseFeeHelper.ConvertWriteDtosToEntities(salesTax.ExciseFees));

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            taxFromRepository.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            await repository.UpdateSalesTaxAsync(taxFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<SalesTaxToRead>> AddSalesTaxAsync(SalesTaxToWrite taxToAdd)
        {
            // 1. Convert dto to domain entity
            var tax = SalesTax.Create(
                taxToAdd.Description,
                taxToAdd.TaxType,
                taxToAdd.Order,
                taxToAdd.TaxIdNumber,
                taxToAdd.PartTaxRate,
                taxToAdd.LaborTaxRate,
                ExciseFeeHelper.ConvertWriteDtosToEntities(taxToAdd.ExciseFees),
                taxToAdd.IsAppliedByDefault,
                taxToAdd.IsTaxable).Value;

            // 2. Add domain entity to repository
            await repository.AddSalesTaxAsync(tax);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new tax from database to consumer after save
            return CreatedAtRoute("GetSalesTaxAsync",
                new { tax.Id },
                SalesTaxHelper.ConvertEntityToReadDto(tax));
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
