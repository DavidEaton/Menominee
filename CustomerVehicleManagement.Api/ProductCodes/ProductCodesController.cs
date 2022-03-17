using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.ProductCodes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCodesController : ControllerBase
    {
        private readonly IProductCodeRepository repository;
        private readonly string BasePath = "/api/productcodes";

        public ProductCodesController(IProductCodeRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/productcodes/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync()
        {
            var results = await repository.GetProductCodeListAsync();
            return Ok(results);
        }

        // api/productcodes/listing/1/1
        [Route("listing")]
        [HttpGet("listing/{mfrid:long}/{scId:long}")]
        public async Task<ActionResult<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodeListAsync(long mfrId, long saleCodeId)
        {
            var results = await repository.GetProductCodeListAsync(mfrId, saleCodeId);
            return Ok(results);
        }

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
        [HttpPut("{mfrcode}/{code}")]
        public async Task<IActionResult> UpdateProductCodeAsync(string mfrCode, string productCode, ProductCodeToWrite pcDto)
        {
            var notFoundMessage = $"Could not find Product Code to update: {pcDto.Name}";

            if (!await repository.ProductCodeExistsAsync(mfrCode, productCode))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var pc = repository.GetProductCodeEntityAsync(mfrCode, productCode).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            pc.Manufacturer = pcDto.Manufacturer;
            pc.Code = pcDto.Code;
            pc.Name = pcDto.Name;
            pc.SaleCode = pcDto.SaleCode;

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            pc.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateProductCodeAsync(pc);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddProductCodeAsync(ProductCodeToWrite pcCreateDto)
        {
            // 1. Convert dto to domain entity
            var pc = new ProductCode()
            {
                Manufacturer = pcCreateDto.Manufacturer,
                Code = pcCreateDto.Code,
                Name = pcCreateDto.Name,
                SaleCode = pcCreateDto.SaleCode
            };

            // 2. Add domain entity to repository
            await repository.AddProductCodeAsync(pc);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return Created(new Uri($"{BasePath}/{pc.Manufacturer.Code}/{pc.Code}", UriKind.Relative), new { ManufacturerCode = pc.Manufacturer.Code, Code = pc.Code });
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
