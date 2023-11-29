using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Taxes
{
    public class SalesTaxesController : BaseApplicationController<SalesTaxesController>
    {
        private readonly ISalesTaxRepository repository;
        public SalesTaxesController(ISalesTaxRepository repository, ILogger<SalesTaxesController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<SalesTaxToReadInList>>> GetListAsync()
        {
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SalesTaxToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(SalesTaxToWrite salesTaxToUpdate)
        {
            var taxFromRepository = await repository.GetEntityAsync(salesTaxToUpdate.Id);
            if (taxFromRepository is null)
            {
                return NotFound($"Could not find Sales Tax to update: {salesTaxToUpdate.Description}");
            }

            if (TaxesAreEqual(taxFromRepository, salesTaxToUpdate))
            {
                return NoContent();
            }

            UpdateSalesTax(salesTaxToUpdate, taxFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }

        private void UpdateSalesTax(SalesTaxToWrite salesTaxToUpdate, SalesTax taxFromRepository)
        {
            taxFromRepository.SetDescription(salesTaxToUpdate.Description);
            taxFromRepository.SetTaxType(salesTaxToUpdate.TaxType);
            taxFromRepository.SetOrder(salesTaxToUpdate.Order);
            taxFromRepository.SetIsAppliedByDefault(salesTaxToUpdate.IsAppliedByDefault);
            taxFromRepository.SetIsTaxable(salesTaxToUpdate.IsTaxable);
            taxFromRepository.SetTaxIdNumber(salesTaxToUpdate.TaxIdNumber);
            taxFromRepository.SetPartTaxRate(salesTaxToUpdate.PartTaxRate);
            taxFromRepository.SetLaborTaxRate(salesTaxToUpdate.LaborTaxRate);
            //taxFromRepository.SetExciseFees(ExciseFeeHelper.ConvertWriteDtosToEntities(salesTax.ExciseFees));
        }

        private static bool TaxesAreEqual(SalesTax entity, SalesTaxToWrite dto)
        {
            return entity.Description == dto.Description
                && entity.TaxType == dto.TaxType
                && entity.Order == dto.Order
                && entity.IsAppliedByDefault == dto.IsAppliedByDefault
                && entity.IsTaxable == dto.IsTaxable
                && entity.TaxIdNumber == dto.TaxIdNumber
                && entity.PartTaxRate == dto.PartTaxRate
                && entity.LaborTaxRate == dto.LaborTaxRate;
        }

        [HttpPost]
        public async Task<ActionResult<SalesTaxToRead>> AddAsync(SalesTaxToWrite taxToAdd)
        {
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

            repository.Add(tax);
            await repository.SaveChangesAsync();

            return Created(
                new Uri($"api/SalesTaxes/{tax.Id}", UriKind.Relative),
                new { tax.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            // TODO - Is this where we should this delete the entries in the SalesTaxTaxableExciseFee table too?
            var taxFromRepository = await repository.GetEntityAsync(id);
            if (taxFromRepository is null)
            {
                return NotFound($"Could not find Sales Tax in the database to delete with Id: {id}.");
            }

            repository.Delete(taxFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
