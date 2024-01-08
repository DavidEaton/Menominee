using Menominee.Domain.Entities;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.SaleCodes
{
    public class SaleCodesController : BaseApplicationController<SaleCodesController>
    {
        private readonly ISaleCodeRepository repository;

        public SaleCodesController(ISaleCodeRepository repository, ILogger<SaleCodesController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SaleCodeToReadInList>>> GetListAsync()
        {
            var result = await repository.GetListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [Route("shopsupplieslist")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SaleCodeShopSuppliesToReadInList>>> GetShopSuppliesListAsync()
        {
            var result = await repository.GetShopSuppliesListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SaleCodeToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<PostResponse>> UpdateAsync(SaleCodeToWrite saleCode)
        {
            var saleCodeFromRepository = await repository.GetEntityAsync(saleCode.Id);

            if (saleCodeFromRepository is null)
                return NotFound($"Could not find Sale Code to update: {saleCode.Name}");

            var saleCodesFromRepository = (await repository.GetListAsync())
                .Select(saleCode => saleCode.Code)
                .ToList();

            var errorMessages = new List<string>();

            if (saleCodeFromRepository.Name != saleCode.Name)
            {
                var nameResult = saleCodeFromRepository.SetName(saleCode.Name);
                if (nameResult.IsFailure)
                    errorMessages.Add(nameResult.Error);
            }

            if (saleCodeFromRepository.Code != saleCode.Code)
            {
                var codeResult = saleCodeFromRepository.SetCode(saleCode.Code, saleCodesFromRepository);
                if (codeResult.IsFailure)
                    errorMessages.Add(codeResult.Error);
            }

            if (saleCodeFromRepository.LaborRate != saleCode.LaborRate)
            {
                var laborRateResult = saleCodeFromRepository.SetLaborRate(saleCode.LaborRate);
                if (laborRateResult.IsFailure)
                    errorMessages.Add(laborRateResult.Error);
            }

            if (saleCodeFromRepository.DesiredMargin != saleCode.DesiredMargin)
            {
                var desiredMarginResult = saleCodeFromRepository.SetDesiredMargin(saleCode.DesiredMargin);
                if (desiredMarginResult.IsFailure)
                    errorMessages.Add(desiredMarginResult.Error);
            }

            if (errorMessages.Any())
            {
                var aggregatedErrorMessages = string.Join(", ", errorMessages);
                return BadRequest(aggregatedErrorMessages);
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(SaleCodeToWrite saleCodeToAdd)
        {
            var saleCodesFromRepository = (await repository.GetListAsync())
                .Select(saleCode => saleCode.Code)
                .ToList();

            var saleCode = SaleCode.Create(
                    saleCodeToAdd.Name,
                    saleCodeToAdd.Code,
                    saleCodeToAdd.LaborRate,
                    saleCodeToAdd.DesiredMargin,
                    SaleCodeShopSuppliesHelper.ConvertWriteDtoToEntity(
                        saleCodeToAdd.ShopSupplies),
                    saleCodesFromRepository).Value;

            repository.Add(saleCode);
            await repository.SaveChangesAsync();

            return Created(new Uri($"api/SaleCodesController/{saleCode.Id}",
                UriKind.Relative),
                new { saleCode.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<PostResponse>> DeleteAsync(long id)
        {
            var saleCode = await repository.GetEntityAsync(id);
            if (saleCode is not null)
                return NotFound($"Could not find Sale Code in the database to delete with Id: {id}.");

            repository.Delete(saleCode);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
