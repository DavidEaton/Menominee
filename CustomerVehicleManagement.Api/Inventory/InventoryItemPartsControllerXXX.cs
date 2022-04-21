using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    //[Route("api/inventoryitems/[controller]")]
    [Route("api/inventoryitems/parts")]
    [ApiController]
    public class InventoryItemPartsControllerXXX : ControllerBase
    {
        private readonly IInventoryItemPartRepositoryXXX partRepository;

        public InventoryItemPartsControllerXXX(IInventoryItemPartRepositoryXXX partRepository)
        {
            this.partRepository = partRepository ?? throw new ArgumentNullException(nameof(partRepository));
        }

        // api/inventoryparts/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryPartToReadInList>>> GetInventoryPartsListAsync()
        {
            var results = await partRepository.GetPartsAsync();
            return Ok(results);
        }

        // api/inventoryparts/1/ABC123
        // ??????? FIX ME -- HOW DOES partRepository LOOKUP PARTS BY MFR + ITEMNUMBER ?????????
        //[HttpGet("{mfrid:long}/{itemnumber}")]
        //public async Task<ActionResult<InventoryPartToRead>> GetInventoryPartAsync(long mfrId, string itemNumber)
        //{
        //    var result = await partRepository.GetPartAsync(mfrId, itemNumber);

        //    if (result == null)
        //        return NotFound();

        //    return result;
        //}

        // api/inventoryparts/1
        [HttpGet("{id:long}", Name = "GetInventoryPartAsync")]
        public async Task<ActionResult<InventoryPartToRead>> GetInventoryPartAsync(long id)
        {
            var result = await partRepository.GetPartAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventoryparts/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryPartAsync(long id, InventoryPartToWrite partToWrite)
        {
            InventoryPartToRead partFromRepository = await partRepository.GetPartAsync(id);

            if (partFromRepository == null)
                return NotFound($"Could not find Inventory Part in the database to update.");

            partRepository.FixTrackingState();

            if (await partRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        [HttpPost]
        public async Task<ActionResult> AddInventoryPartAsync(InventoryPartToWrite partToAdd)
        {
            InventoryItemPart part = null;

            await partRepository.AddPartAsync(part);

            if (!await partRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {partToAdd}.");

            InventoryPartToRead partFromRepository = await partRepository.GetPartAsync(part.Id);

            if (partFromRepository == null)
                return BadRequest($"Failed to add {partToAdd}.");

            return CreatedAtRoute("GetInventoryPartAsync",
                                  new { id = partFromRepository.Id },
                                  partFromRepository);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryPartAsync(long id)
        {
            var partFromRepository = await partRepository.GetPartAsync(id);
            if (partFromRepository == null)
                return NotFound($"Could not find Inventory Part in the database to delete with Id = {id}.");

            await partRepository.DeletePartAsync(id);
            await partRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
