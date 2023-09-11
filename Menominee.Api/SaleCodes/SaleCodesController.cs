using Menominee.Api.Common;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.SaleCodes
{
    public class SaleCodesController : BaseApplicationController<SaleCodesController>
    {
        private readonly ISaleCodeRepository repository;
        private readonly string BasePath = "/api/salecodes";

        public SaleCodesController(ISaleCodeRepository repository, ILogger<SaleCodesController> logger) : base(logger)
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

        // api/salecodes/shopsupplieslist
        [Route("shopsupplieslist")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SaleCodeShopSuppliesToReadInList>>> GetSaleCodeShopSuppliesListAsync()
        {
            var results = await repository.GetSaleCodeShopSuppliesListAsync();
            return Ok(results);
        }

        // api/salecodes/1
        [HttpGet("{id:long}", Name = "GetSaleCodeAsync")]
        public async Task<ActionResult<SaleCodeToRead>> GetSaleCodeAsync(long id)
        {
            var result = await repository.GetSaleCodeAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/salecodes/1
        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateSaleCodeAsync(long id, SaleCodeToWrite saleCode)
        {
            var notFoundMessage = $"Could not find Sale Code to update: {saleCode.Name}";

            if (!await repository.SaleCodeExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var saleCodeFromRepository = await repository.GetSaleCodeEntityAsync(id);
            var saleCodesFromRepository = (await repository.GetSaleCodeListAsync())
                .Select(saleCode => saleCode.Code)
                .ToList();

            // 2) Update domain entity with data in data transfer object(DTO)
            var nameResult = saleCodeFromRepository.SetName(saleCode.Name);
            if (nameResult.IsFailure) return BadRequest(nameResult.Error);

            var codeResult = saleCodeFromRepository.SetCode(saleCode.Code, saleCodesFromRepository);
            if (codeResult.IsFailure) return BadRequest(codeResult.Error);

            var laborRateResult = saleCodeFromRepository.SetLaborRate(saleCode.LaborRate);
            if (laborRateResult.IsFailure) return BadRequest(laborRateResult.Error);

            var desiredMarginResult = saleCodeFromRepository.SetDesiredMargin(saleCode.DesiredMargin);
            if (desiredMarginResult.IsFailure) return BadRequest(desiredMarginResult.Error);

            //saleCodeFromRepository.SetShopSupplies(saleCode.ShopSupplies);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddSaleCodeAsync(SaleCodeToWrite saleCodeToAdd)
        {
            SaleCode saleCode = null;
            var saleCodesFromRepository = (await repository.GetSaleCodeListAsync())
                .Select(saleCode => saleCode.Code)
                .ToList();

            // 1. Convert dto to domain entity
            var result = SaleCode.Create(
                    saleCodeToAdd.Name,
                    saleCodeToAdd.Code,
                    saleCodeToAdd.LaborRate,
                    saleCodeToAdd.DesiredMargin,
                    SaleCodeShopSuppliesHelper.ConvertWriteDtoToEntity(
                        saleCodeToAdd.ShopSupplies),
                    saleCodesFromRepository);

            if (result.IsFailure) return BadRequest(result.Error);

            saleCode = result.Value;

            // 2. Add domain entity to repository
            await repository.AddSaleCodeAsync(saleCode);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return Created(new Uri($"{BasePath}/{saleCode.Id}", UriKind.Relative), new { id = saleCode.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteSaleCodeAsync(long id)
        {
            var scFromRepository = await repository.GetSaleCodeAsync(id);
            if (scFromRepository == null)
                return NotFound($"Could not find Sale Code in the database to delete with Id: {id}.");

            await repository.DeleteSaleCodeAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
