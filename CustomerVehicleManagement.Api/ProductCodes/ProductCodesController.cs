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

        // api/productcodes/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync()
        {
            var results = await repository.GetProductCodesInListAsync();
            return Ok(results);
        }

        // api/productcodes/listing/1
        [Route("listing")]
        [HttpGet("listing/{mfrid:long}")]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync(long mfrId)
        {
            var results = await repository.GetProductCodesInListAsync(mfrId);
            return Ok(results);
        }

        // api/productcodes/listing/1/1
        //[Route("listing")]
        //[HttpGet("listing/{mfrid:long}/{scId:long}")]
        //public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync(long mfrId, long saleCodeId)
        //{
        //    var results = await repository.GetProductCodesInListAsync(mfrId, saleCodeId);
        //    return Ok(results);
        //}

        // api/productcodes/xyz/123
        [HttpGet("{mfrcode}/{code}")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(string mfrCode, string productCode)
        {
            var result = await repository.GetProductCodeAsync(mfrCode, productCode);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/productcodes/xyz/123
        [HttpGet("{id:long}", Name = "GetProductCodeAsync")]
        public async Task<ActionResult<ProductCodeToRead>> GetProductCodeAsync(long id)
        {
            var result = await repository.GetProductCodeAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/productcodes/xyz/123
        [HttpPut("{mfrcode}/{code}")]
        public async Task<IActionResult> UpdateProductCodeAsync(string mfrCode, string productCode, ProductCodeToWrite pcDto)
        {
            var notFoundMessage = $"Could not find Product Code to update: {pcDto.Name}";

            if (!await repository.ProductCodeExistsAsync(mfrCode, productCode))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var productCodeFromRepository = await repository.GetProductCodeEntityAsync(mfrCode, productCode);

            // 2) Update domain entity with data in data transfer object(DTO)
            //pc.Manufacturer = pcDto.Manufacturer;
            //pc.Code = pcDto.Code;
            //pc.Name = pcDto.Name;
            //pc.SaleCode = pcDto.SaleCode;
            ProductCodeHelper.CopyWriteDtoToEntity(pcDto, productCodeFromRepository);

            await repository.UpdateProductCodeAsync(productCodeFromRepository);

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

        [HttpDelete("{mfrcode}/{code}")]
        public async Task<IActionResult> DeleteProductCodeAsync(string mfrCode, string code)
        {
            var pcFromRepository = await repository.GetProductCodeAsync(mfrCode, code);
            if (pcFromRepository == null)
                return NotFound($"Could not find Product Code in the database to delete with Id: {code}.");

            await repository.DeleteProductCodeAsync(mfrCode, code);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
