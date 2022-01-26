using MenomineePlayWASM.Server.Repository.Inventory;
using MenomineePlayWASM.Shared.Dtos;
using MenomineePlayWASM.Shared.Dtos.Inventory;
using MenomineePlayWASM.Shared.Entities.Inventory;
using MenomineePlayWASM.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Controllers.Inventory
{
    [ApiController]
    [Route("api/inventory/[controller]")]
    public class InventoryItemsController: ControllerBase
    {
        private readonly IInventoryItemRepository repository;
        private readonly string BasePath = "/api/inventory/inventoryitems";

        public InventoryItemsController(IInventoryItemRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/inventory/inventoryitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetItemsListAsync()
        {
            var results = await repository.GetItemListAsync();
            return Ok(results);
        }

        // api/inventory/inventoryitems
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToRead>>> GetItemsAsync()
        {
            var result = await repository.GetItemsAsync();
            return Ok(result);
        }

        // api/inventory/inventoryitems/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<InventoryItemToRead>> GetItemAsync(long id)
        {
            var result = await repository.GetItemAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }


        // api/inventory/inventoryitems/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateItemAsync(long id, InventoryItemToWrite itemUpdateDto)
        {
            if (!await repository.ItemExistsAsync(id))
                return NotFound($"Could not find part # {itemUpdateDto.PartNumber} to update.");

            //1) Get domain entity from repository
            var item = repository.GetItemEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            //item.Manufacturer = 
            item.PartNumber = itemUpdateDto.PartNumber;
            item.Description = itemUpdateDto.Description;
            item.PartType = itemUpdateDto.PartType;
            item.Retail = itemUpdateDto.Retail;
            item.Cost = itemUpdateDto.Cost;
            item.Core = itemUpdateDto.Core;
            item.Labor = itemUpdateDto.Labor;
            item.OnHand = itemUpdateDto.OnHand;

            // Update the objects ObjectState and synch the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            item.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateItemAsync(item);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemToRead>> AddItemAsync(InventoryItemToWrite itemCreateDto)
        {
            // 1. Convert dto to domain entity
            var item = new InventoryItem()
            {
                // Manufacturer = 
                PartNumber = itemCreateDto.PartNumber,
                Description = itemCreateDto.Description,
                PartType = itemCreateDto.PartType,
                Retail = itemCreateDto.Retail,
                Cost = itemCreateDto.Cost,
                Core = itemCreateDto.Core,
                Labor = itemCreateDto.Labor,
                OnHand = itemCreateDto.OnHand
            };

            // 2. Add domain entity to repository
            await repository.AddItemAsync(item);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new Id from database to consumer after save
            return Created(new Uri($"{BasePath}/{item.Id}", UriKind.Relative), new { id = item.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteItemAsync(long id)
        {
            var itemFromRepository = await repository.GetItemAsync(id);
            if (itemFromRepository == null)
                return NotFound($"Could not find part in the database to delete with id of {id}.");

            await repository.DeleteItemAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
