using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryLaborController : ControllerBase
    {
        private readonly IInventoryLaborRepository laborRepository;

        public InventoryLaborController(IInventoryLaborRepository laborRepository)
        {
            this.laborRepository = laborRepository ?? throw new ArgumentNullException(nameof(laborRepository));
        }

        // api/inventorylabor/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryLaborToReadInList>>> GetInventoryLaborListAsync()
        {
            var results = await laborRepository.GetLaborsAsync();
            return Ok(results);
        }

        // api/inventorylabor/1/ABC123
        // ??????? FIX ME -- HOW DOES laborRepository LOOKUP PARTS BY MFR + ITEMNUMBER ?????????
        //[HttpGet("{mfrid:long}/{itemnumber}")]
        //public async Task<ActionResult<InventoryLaborToRead>> GetInventoryLaborAsync(long mfrId, string itemNumber)
        //{
        //    var result = await laborRepository.GetLaborAsync(mfrId, itemNumber);

        //    if (result == null)
        //        return NotFound();

        //    return result;
        //}

        // api/inventorylabor/1
        [HttpGet("{id:long}", Name = "GetInventoryLaborAsync")]
        public async Task<ActionResult<InventoryLaborToRead>> GetInventoryLaborAsync(long id)
        {
            var result = await laborRepository.GetLaborAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventorylabor/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryLaborAsync(long id, InventoryLaborToWrite laborToWrite)
        {
            InventoryLaborToRead laborFromRepository = await laborRepository.GetLaborAsync(id);

            if (laborFromRepository == null)
                return NotFound($"Could not find Inventory Labor in the database to update.");

            laborRepository.FixTrackingState();

            if (await laborRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        [HttpPost]
        public async Task<ActionResult> AddInventoryPartAsync(InventoryLaborToWrite laborToAdd)
        {
            InventoryLabor labor = null;

            await laborRepository.AddLaborAsync(labor);

            if (!await laborRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {laborToAdd}.");

            InventoryLaborToRead laborFromRepository = await laborRepository.GetLaborAsync(labor.Id);

            if (laborFromRepository == null)
                return BadRequest($"Failed to add {laborToAdd}.");

            return CreatedAtRoute("GetInventoryLaborAsync",
                                  new { id = laborFromRepository.Id },
                                  laborFromRepository);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryPartAsync(long id)
        {
            var partFromRepository = await laborRepository.GetLaborAsync(id);
            if (partFromRepository == null)
                return NotFound($"Could not find Inventory Labor in the database to delete with Id = {id}.");

            await laborRepository.DeleteLaborAsync(id);
            await laborRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
