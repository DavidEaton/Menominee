using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.SaleCodes
{
    public class SaleCodesController : ApplicationController
    {
        private readonly ISaleCodeRepository repository;
        //private readonly string BasePath = "/api/salecodes";

        public SaleCodesController(ISaleCodeRepository repository)
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

        // api/salecodes/a
        [HttpGet("{code}")]
        public async Task<ActionResult<SaleCodeToRead>> GetSaleCodeAsync(string code)
        {
            var result = await repository.GetSaleCodeAsync(code);

            if (result == null)
                return NotFound();

            return result;
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
        public async Task<IActionResult> UpdateSaleCodeAsync(long id, SaleCodeToWrite scDto)
        {
            var notFoundMessage = $"Could not find Sale Code to update: {scDto.Name}";

            if (!await repository.SaleCodeExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var sc = repository.GetSaleCodeEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            SaleCodeHelper.CopySaleCode(scDto, sc); 

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            sc.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            await repository.UpdateSaleCodeAsync(sc);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddSaleCodeAsync(SaleCodeToWrite scCreateDto)
        {
            // 1. Convert dto to domain entity
            //var sc = new SaleCode()
            //{
            //    Code = scCreateDto.Code,
            //    Name = scCreateDto.Name
            //};
            var sc = SaleCodeHelper.CreateSaleCode(scCreateDto);

            // 2. Add domain entity to repository
            await repository.AddSaleCodeAsync(sc);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Code from database to consumer after save
            return CreatedAtRoute("GetSaleCodeAsync",
                new
                {
                    Id = sc.Id
                },
                SaleCodeHelper.CreateSaleCode(sc));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteSaleCodeAsync(long id)
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
