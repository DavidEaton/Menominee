using Menominee.Api.Features.Manufacturers;
using Menominee.Api.Features.SaleCodes;
using Menominee.Common.Http;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.ProductCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.ProductCodes
{
    public class ProductCodesController : BaseApplicationController<ProductCodesController>
    {
        private readonly IProductCodeRepository repository;
        private readonly IManufacturerRepository manufacturersRepository;
        private readonly ISaleCodeRepository saleCodesRepository;
        public ProductCodesController(
            IProductCodeRepository repository,
             IManufacturerRepository manufacturersRepository,
             ISaleCodeRepository saleCodesRepository,
             ILogger<ProductCodesController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.manufacturersRepository = manufacturersRepository ??
                throw new ArgumentNullException(nameof(manufacturersRepository));

            this.saleCodesRepository = saleCodesRepository ??
                throw new ArgumentNullException(nameof(saleCodesRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetListAsync([FromQuery] long? manufacturerId = null, long? saleCodeId = null)
        {
            return Ok(await repository.GetListAsync(manufacturerId, saleCodeId));
        }

        [HttpGet("{manufacturerid}/{productcode}")]
        public async Task<ActionResult<ProductCodeToRead>> GetAsync(long manufacturerId, string productCode)
        {
            var result = await repository.GetAsync(manufacturerId, productCode);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductCodeToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(ProductCodeToWrite productCodeFromCaller)
        {
            var productCodeFromRepository = await repository.GetEntityAsync(productCodeFromCaller.Id);

            if (productCodeFromRepository is null)
                return NotFound($"Could not find Product Code to update: {productCodeFromCaller.Name}");

            await UpdateProductCodeAsync(productCodeFromRepository, productCodeFromCaller);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private async Task UpdateProductCodeAsync(ProductCode productCodeFromRepository, ProductCodeToWrite productCodeFromCaller)
        {
            if (productCodeFromRepository.Name != productCodeFromCaller.Name)
                productCodeFromRepository.SetName(productCodeFromCaller.Name);

            if (productCodeFromRepository.Manufacturer.Id != productCodeFromCaller.Manufacturer.Id)
                productCodeFromRepository.SetManufacturer(
                    await manufacturersRepository.GetEntityAsync(productCodeFromCaller.Manufacturer.Id));

            if (productCodeFromRepository.SaleCode.Id != productCodeFromCaller.SaleCode.Id)
                productCodeFromRepository.SetSaleCode(
                    await saleCodesRepository.GetEntityAsync(productCodeFromCaller.SaleCode.Id));

            if (productCodeFromRepository.Code != productCodeFromCaller.Code)
                productCodeFromRepository.SetCode(
                    productCodeFromCaller.Code, repository.GetManufacturerCodes());
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(ProductCodeToWrite productCodeToAdd)
        {
            // no need to validate it here again, just call .Value right away
            var failureMessage = $"Could not add new Product Code: {productCodeToAdd?.Code}.";
            var manufacturerCodes = repository.GetManufacturerCodes();
            var manufacturer = await manufacturersRepository.GetEntityAsync(
                productCodeToAdd.Manufacturer.Id);
            var saleCode = await saleCodesRepository.GetEntityAsync(
                productCodeToAdd.SaleCode.Id);

            if (manufacturer is null)
                return NotFound(failureMessage);

            var productCode = ProductCode.Create(
                manufacturer,
                productCodeToAdd.Code,
                productCodeToAdd.Name,
                manufacturerCodes,
                saleCode).Value;

            repository.Add(productCode);
            await repository.SaveChangesAsync();

            return Created(new Uri($"api/ProductCodesController/{productCode.Id}",
                UriKind.Relative),
                new { productCode.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var productCode = await repository.GetEntityAsync(id);
            if (productCode is null)
                return NotFound($"Could not find Product Code in the database to delete with Id: {id}.");

            repository.Delete(productCode);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
