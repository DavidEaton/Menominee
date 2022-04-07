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
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTiresController : ControllerBase
    {
        private readonly IInventoryTireRepository tireRepository;

        public InventoryTiresController(IInventoryTireRepository tireRepository)
        {
            this.tireRepository = tireRepository ?? throw new ArgumentNullException(nameof(tireRepository));
        }

        // api/inventorytires/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryTireToReadInList>>> GetInventoryTiresListAsync()
        {
            var results = await tireRepository.GetTiresAsync();
            return Ok(results);
        }

        // api/inventorytires/1/ABC123
        // ??????? FIX ME -- HOW DOES tireRepository LOOKUP PARTS BY MFR + ITEMNUMBER ?????????
        //[HttpGet("{mfrid:long}/{itemnumber}")]
        //public async Task<ActionResult<InventoryTireToRead>> GetInventoryTireAsync(long mfrId, string itemNumber)
        //{
        //    var result = await tireRepository.GetTireAsync(mfrId, itemNumber);

        //    if (result == null)
        //        return NotFound();

        //    return result;
        //}

        // api/inventorytires/1
        [HttpGet("{id:long}", Name = "GetInventoryTireAsync")]
        public async Task<ActionResult<InventoryTireToRead>> GetInventoryTireAsync(long id)
        {
            var result = await tireRepository.GetTireAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventorytires/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryTireAsync(long id, InventoryTireToWrite tireToWrite)
        {
            InventoryTireToRead tireFromRepository = await tireRepository.GetTireAsync(id);

            if (tireFromRepository == null)
                return NotFound($"Could not find Inventory Tire in the database to update.");

            tireRepository.FixTrackingState();

            if (await tireRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        [HttpPost]
        public async Task<ActionResult> AddInventoryTireAsync(InventoryTireToWrite tireToAdd)
        {
            InventoryTire tire = null;

            await tireRepository.AddTireAsync(tire);

            if (!await tireRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {tireToAdd}.");

            InventoryTireToRead tireFromRepository = await tireRepository.GetTireAsync(tire.Id);

            if (tireFromRepository == null)
                return BadRequest($"Failed to add {tireToAdd}.");

            return CreatedAtRoute("GetInventoryTireAsync",
                                  new { id = tireFromRepository.Id },
                                  tireFromRepository);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryTireAsync(long id)
        {
            var tireFromRepository = await tireRepository.GetTireAsync(id);
            if (tireFromRepository == null)
                return NotFound($"Could not find Inventory Tire in the database to delete with Id = {id}.");

            await tireRepository.DeleteTireAsync(id);
            await tireRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
