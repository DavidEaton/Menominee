using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    ////[Route("api/[controller]")]
    //[Route("api/payables/[controller]")]
    //[ApiController]
    public class VendorsController : ApplicationController
    {
        private readonly IVendorRepository repository;
        private readonly string BasePath = "/api/payables/vendors";

        public VendorsController(IVendorRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/vendors/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VendorToReadInList>>> GetVendorsListAsync()
        {
            var results = await repository.GetVendorsListAsync();
            return Ok(results);
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
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Set entity's TrackingState to Modified
                4) FixTrackingState: moves entity state tracking back out of
                the object and into the context to track entity state in this
                disconnected application. In other words, sych the EF Change
                Tracker with our disconnected entitys TrackingState
                5) Save changes
                6) return NoContent()
            */
            // VK: here, the logic should be:
            // 1. Get the Organization entity (not DTO) from the DB
            // 3. Update the corresponding fields in the Organization
            // 4. Save back to the DB

            var notFoundMessage = $"Could not find Vendor to update: {vendor.Name}";

            if (!await repository.VendorExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            // 2) Update domain entity with data in data transfer object(DTO)
            //vendor.Id = vendorUpdateDto.Id;
            vendorFromRepository.SetVendorCode(vendor.VendorCode);
            vendorFromRepository.SetName(vendor.Name);
            vendorFromRepository.SetIsActive(vendor.IsActive);

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            vendorFromRepository.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            /* Returning the updated resource is acceptible, for example:
                 return Ok(personFromRepository);
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store
               (which they are not in this case).

               Instead, our app will:
                 return NoContent();
               ... and let the caller decide whether to get the updated resource,
               which is also acceptible. The HTTP specification (RFC 2616) has a
               number of recommendations that are applicable:
            HTTP status code 200 OK for a successful PUT of an update to an existing resource. No response body needed.
            HTTP status code 201 Created for a successful PUT of a new resource alhole11TELalhole11TELalhole11TEL
            HTTP status code 409 Conflict for a PUT that is unsuccessful due to a 3rd-party modification
            HTTP status code 400 Bad Request for an unsuccessful PUT
            */
            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddVendorAsync(VendorToWrite vendorToAdd)
        {
            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute; most of our controllers inherit from ApplicationController,
                which has the [ApiController] attribute. With [ApiController] attribute applied,
                an automatic HTTP 400 response containing error details is returned when model
                state is invalid.*/

            /* Controller Pattern:
                1. Convert data contract/data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Return to consumer */

            // 1. Convert dto to domain entity
            var vendor = VendorHelper.ConvertWriteDtoToEntity(vendorToAdd);

            // 2. Add domain entity to repository
            await repository.AddVendorAsync(vendor);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new id and route to new resource after save
            return Created(new Uri($"{BasePath}/{vendor.Id}",
                               UriKind.Relative),
                               new { vendor.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteVendorAsync(long id)
        {
            /* Delete Pattern in Controllers:
             1) Get domain entity from repository
             2) Call repository.Delete(), which removes entity from context
             3) Save changes
             4) return Ok()
         */
            var vendorFromRepository = await repository.GetVendorEntityAsync(id);

            if (vendorFromRepository is null)
                return NotFound($"Could not find Vendor in the database to delete with Id: {id}.");

            repository.DeleteVendor(vendorFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
