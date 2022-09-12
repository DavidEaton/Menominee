using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CustomerVehicleManagement.Api.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceItemsController : ControllerBase
    {
        private readonly IMaintenanceItemRepository itemRepository;

        public MaintenanceItemsController(IMaintenanceItemRepository itemRepository)
        {
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        // api/maintenanceitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MaintenanceItemToReadInList>>> GetMaintenanceItemsListAsync()
        {
            var results = await itemRepository.GetItemsInListAsync();

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/maintenanceitems/1
        [HttpGet("{id:long}", Name = "GetMaintenanceItemAsync")]
        public async Task<ActionResult<MaintenanceItemToRead>> GetMaintenanceItemAsync(long id)
        {
            var result = await itemRepository.GetItemAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/maintenanceitems/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateMaintenanceItemAsync(MaintenanceItemToWrite itemToWrite, long id)
        {
            if (!await itemRepository.ItemExistsAsync(id))
                return NotFound($"Could not find Maintenance Item # {id} to update.");

            var item = await itemRepository.GetItemEntityAsync(id);

            MaintenanceItemHelper.CopyWriteDtoToEntity(itemToWrite, item);

            item.SetTrackingState(TrackingState.Modified);
            item.Item.SetTrackingState(TrackingState.Unchanged);
            item.Item.Manufacturer?.SetTrackingState(TrackingState.Unchanged);
            item.Item.ProductCode?.SetTrackingState(TrackingState.Unchanged);
            itemRepository.FixTrackingState();

            if (await itemRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceItemToRead>> AddMaintenanceItemAsync(MaintenanceItemToWrite itemToWrite)
        {
            MaintenanceItem item = MaintenanceItemHelper.ConvertWriteDtoToEntity(itemToWrite);
            await itemRepository.AddItemAsync(item);
            await itemRepository.SaveChangesAsync();

            return CreatedAtRoute("GetMaintenanceItemAsync",
                                  new { id = item.Id },
                                  MaintenanceItemHelper.ConvertEntityToReadDto(item));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteMaintenanceItemAsync(long id)
        {
            var notFoundMessage = $"Could not find Maintenance Item in the database to delete with Id = {id}.";

            MaintenanceItem itemFromRepository = await itemRepository.GetItemEntityAsync(id);

            if (itemFromRepository == null)
                return NotFound(notFoundMessage);

            itemRepository.DeleteItem(itemFromRepository);
            await itemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}