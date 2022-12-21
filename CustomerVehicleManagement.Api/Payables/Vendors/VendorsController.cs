using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    public class VendorsController : ApplicationController
    {
        private readonly IVendorRepository repository;
        private readonly string BasePath = "/api/payables/vendors";

        public VendorsController(IVendorRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/vendors
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorToRead>>> GetVendorsAsync()
        {
            var result = await repository.GetVendorsAsync();
            return Ok(result);
        }

        // api/vendors/1
        [HttpGet("{id:long}", Name = "GetVendorAsync")]
        public async Task<ActionResult<VendorToRead>> GetVendorAsync(long id)
        {
            var result = await repository.GetVendorAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/vendors/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateVendorAsync(long id, VendorToWrite vendor)
        {
            var notFoundMessage = $"Could not find Vendor to update: {vendor.Name}";

            if (!await repository.VendorExistsAsync(id))
                return NotFound(notFoundMessage);

            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            vendorFromRepository.SetVendorCode(vendor.VendorCode);
            vendorFromRepository.SetName(vendor.Name);

            if (vendorFromRepository.IsActive.HasValue)
            {
                if (vendorFromRepository.IsActive.Value.Equals(true))
                    vendorFromRepository.Enable();

                if (vendorFromRepository.IsActive.Value.Equals(false))
                    vendorFromRepository.Disable();
            }

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddVendorAsync(VendorToWrite vendorToAdd)
        {
            var vendor = VendorHelper.ConvertWriteDtoToEntity(vendorToAdd);

            await repository.AddVendorAsync(vendor);

            await repository.SaveChangesAsync();

            return Created(new Uri($"{BasePath}/{vendor.Id}",
                               UriKind.Relative),
                               new { vendor.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteVendorAsync(long id)
        {
            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            if (vendorFromRepository is null)
                return NotFound($"Could not find Vendor in the database to delete with Id: {id}.");

            repository.DeleteVendor(vendorFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
