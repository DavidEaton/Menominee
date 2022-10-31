using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.SaleCodes;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.ProductCodes
{
    public class ProductCodesController : ApplicationController
    {
        private readonly string BasePath = "/api/productcodes";
        private readonly IProductCodeRepository repository;
        private readonly IManufacturerRepository manufacturersRepository;
        private readonly ISaleCodeRepository saleCodesRepository;
        public ProductCodesController(IProductCodeRepository repository, IManufacturerRepository manufacturersRepository, ISaleCodeRepository saleCodesRepository)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            this.manufacturersRepository = manufacturersRepository ??
                throw new ArgumentNullException(nameof(manufacturersRepository));

            this.saleCodesRepository = saleCodesRepository ??
                throw new ArgumentNullException(nameof(saleCodesRepository));
        }

        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync()
        {
            var results = await repository.GetProductCodesInListAsync();
            return Ok(results);
        }

        [Route("listing")]
        [HttpGet("listing/{manufacturerId:long}")]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync(long manufacturerId)
        {
            var results = await repository.GetProductCodesInListAsync(manufacturerId);
            return Ok(results);
        }

        [HttpGet("{manufacturercode}/{code}")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(string manufacturerCode, string productCode)
        {
            var result = await repository.GetProductCodeAsync(manufacturerCode, productCode);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/productcodes/123
        [HttpGet("{id:long}", Name = "GetProductCodeAsync")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(long id)
        {
            var result = await repository.GetProductCodeAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/productcodes/123
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
                var manufacturerCodes = repository.GetManufacturerCodes();

                var resultOrError = productCodeFromRepository.SetManufacturer(
                    await manufacturersRepository.GetManufacturerEntityAsync(productCodeFromCaller.Manufacturer.Id),
                    manufacturerCodes);

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
        public async Task<ActionResult<ProductCodeToRead>> AddProductCodeAsync(ProductCodeToWrite productCodeToAdd)
        {
            var failureMessage = $"Could not add new Product Code: {productCodeToAdd?.Code}.";
            var manufacturerCodes = repository.GetManufacturerCodes();
            var manufacturer = await manufacturersRepository.GetManufacturerEntityAsync(productCodeToAdd.Manufacturer.Id);
            var saleCode = await saleCodesRepository.GetSaleCodeEntityAsync(productCodeToAdd.SaleCode.Id);

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
