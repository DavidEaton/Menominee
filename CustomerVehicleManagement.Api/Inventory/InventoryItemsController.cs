//using CustomerVehicleManagement.Api.Configurations.Inventory;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class InventoryItemsController : ApplicationController
    {
        private readonly IInventoryItemRepository itemRepository;

        public InventoryItemsController(IInventoryItemRepository itemRepository)
        {
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        // api/inventoryitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync()
        {
            var results = await itemRepository.GetItemsInListAsync();
            return Ok(results);
        }

        // api/inventoryitems/listing/1
        [Route("listing")]
        [HttpGet("listing/{mfrid:long}")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync(long mfrId)
        {
            var results = await itemRepository.GetItemsInListAsync(mfrId);
            return Ok(results);
        }

        // api/inventoryitems/1/ABC123
        [HttpGet("{mfrid:long}/{itemnumber}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long mfrId, string itemNumber)
        {
            var result = await itemRepository.GetItemAsync(mfrId, itemNumber);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventoryitems/1
        [HttpGet("{id:long}", Name = "GetInventoryItemAsync")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long id)
        {
            var result = await itemRepository.GetItemAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventoryitems/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryItemAsync(long id, InventoryItemToWrite itemToWrite)
        {
            if (!await itemRepository.ItemExistsAsync(id))
                return NotFound($"Could not find Inventory Item # {id} to update.");

            //InventoryItemToRead itemFromRepository = await itemRepository.GetItemAsync(id);

            //1) Get domain entity from repository
            var item = itemRepository.GetItemEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            InventoryItemHelper.CopyWriteDtoToEntity(itemToWrite, item);

            // Update the objects ObjectState and synch the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            item.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            itemRepository.FixTrackingState();

            if (await itemRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemToRead>> AddInventoryItemAsync(InventoryItemToWrite itemToAdd)
        {
            InventoryItem item = null;

            if (itemToAdd.ItemType == InventoryItemType.Part)
                item = new(InventoryPartHelper.Transform(itemToAdd.Part));
            else if (itemToAdd.ItemType == InventoryItemType.Labor)
                item = new(InventoryLaborHelper.Transform(itemToAdd.Labor));
            else if (itemToAdd.ItemType == InventoryItemType.Tire)
                item = new(InventoryTireHelper.Transform(itemToAdd.Tire));

            item.ManufacturerId = itemToAdd.ManufacturerId;
            item.ItemNumber = itemToAdd.ItemNumber;
            item.Description = itemToAdd.Description;
            item.ProductCodeId = itemToAdd.ProductCodeId;

            await itemRepository.AddItemAsync(item);

            //if (!await itemRepository.SaveChangesAsync())
            //    return BadRequest($"Failed to add {itemToAdd}.");

            //InventoryItemToRead itemFromRepository = await itemRepository.GetItemAsync(item.Id);

            //if (itemFromRepository == null)
            //    return BadRequest($"Failed to add {itemToAdd}.");

            //return CreatedAtRoute("GetInventoryItemAsync",
            //                      new { id = itemFromRepository.Id },
            //                      itemFromRepository);
            //return Created(new Uri($"{BasePath}/{item.ManufacturerId}/{item.ItemNumber}", UriKind.Relative), new { ManufacturerId = item.ManufacturerId, ItemNumber = item.ItemNumber });

            await itemRepository.SaveChangesAsync();

            return CreatedAtRoute("GetInventoryItemAsync",
                                  new { id = item.Id },
                                  InventoryItemHelper.Transform(item));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryItemAsync(long id)
        {
            var itemFromRepository = await itemRepository.GetItemAsync(id);
            if (itemFromRepository == null)
                return NotFound($"Could not find Inventory Item in the database to delete with Id = {id}.");

            await itemRepository.DeleteItemAsync(id);
            await itemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
