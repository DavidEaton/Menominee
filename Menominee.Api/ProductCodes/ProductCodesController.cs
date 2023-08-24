using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Menominee.Api.Common;
using Menominee.Api.Manufacturers;
using Menominee.Api.SaleCodes;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.ProductCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menominee.Api.ProductCodes
{
    public class ProductCodesController : BaseApplicationController<ProductCodesController>
    {
        private readonly string BasePath = "/api/productcodes";
        private readonly IProductCodeRepository repository;
        private readonly IManufacturerRepository manufacturersRepository;
        private readonly ISaleCodeRepository saleCodesRepository;
        public ProductCodesController(
            IProductCodeRepository repository
            , IManufacturerRepository manufacturersRepository
            , ISaleCodeRepository saleCodesRepository
            , ILogger<ProductCodesController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.manufacturersRepository = manufacturersRepository ??
                throw new ArgumentNullException(nameof(manufacturersRepository));

            this.saleCodesRepository = saleCodesRepository ??
                throw new ArgumentNullException(nameof(saleCodesRepository));
        }

        [Route("listing")]
        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync([FromQuery] long? manufacturerId = null, long? saleCodeId = null)
        {
            return Ok(await repository.GetProductCodesInListAsync(manufacturerId, saleCodeId));
        }

        [HttpGet("{manufacturerid}/{productcode}")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(long manufacturerId, string productCode)
        {
            var result = await repository.GetProductCodeAsync(manufacturerId, productCode);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(long id)
        {
            var result = await repository.GetProductCodeAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProductCodeAsync(long id, ProductCodeToWrite productCodeFromCaller)
        {
            //1) Get domain entity from repository
            var productCodeFromRepository = await repository.GetProductCodeEntityAsync(id);

            if (productCodeFromRepository is null)
                return NotFound($"Could not find Product Code to update: {productCodeFromCaller.Name}");

            // 2) Update domain entity ProductCode with data in
            // data contract/transfer object(DTO).
            // Update each member of ProductCode, or return a Bad
            // Resuest response with error message, in case of Failure:
            if (productCodeFromRepository.Name != productCodeFromCaller.Name)
            {
                var resultOrError = productCodeFromRepository.SetName(productCodeFromCaller.Name);

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (productCodeFromRepository.Manufacturer.Id != productCodeFromCaller.Manufacturer.Id)
            {
                var resultOrError = productCodeFromRepository.SetManufacturer(
                    await manufacturersRepository.GetManufacturerEntityAsync(productCodeFromCaller.Manufacturer.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (productCodeFromRepository.SaleCode.Id != productCodeFromCaller.SaleCode.Id)
            {
                var resultOrError = productCodeFromRepository.SetSaleCode(
                    await saleCodesRepository.GetSaleCodeEntityAsync(productCodeFromCaller.SaleCode.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (productCodeFromRepository.Code != productCodeFromCaller.Code)
            {
                var manufacturerCodes = repository.GetManufacturerCodes();

                var resultOrError = productCodeFromRepository.SetCode(productCodeFromCaller.Code, manufacturerCodes);

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductCodeAsync(ProductCodeToWrite productCodeToAdd)
        {
            var failureMessage = $"Could not add new Product Code: {productCodeToAdd?.Code}.";
            var manufacturerCodes = repository.GetManufacturerCodes();
            var manufacturer = await manufacturersRepository.GetManufacturerEntityAsync(
                productCodeToAdd.Manufacturer.Id);
            var saleCode = await saleCodesRepository.GetSaleCodeEntityAsync(
                productCodeToAdd.SaleCode.Id);

            if (manufacturer is null)
                return NotFound(failureMessage);

            // 1. Convert dto to domain entity
            var resultOrError = ProductCode.Create(
                manufacturer,
                productCodeToAdd.Code,
                productCodeToAdd.Name,
                manufacturerCodes,
                saleCode);

            if (resultOrError.IsFailure)
                return BadRequest(resultOrError.Error);

            // 2. Add domain entity to repository
            repository.AddProductCode(resultOrError.Value);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return to caller
            return Created(
                new Uri($"{BasePath}/{resultOrError.Value.Id}",
                UriKind.Relative),
                new
                {
                    resultOrError.Value.Id
                });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProductCodeAsync(long id)
        {
            if (await repository.ProductCodeExistsAsync(id) == false)
                return NotFound($"Could not find Product Code in the database to delete with Id: {id}.");

            await repository.DeleteProductCodeAsync(id);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
