using CustomerVehicleManagement.Domain.Entities;
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
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IInventoryItemRepository repository;
        private readonly string BasePath = "/api/inventoryitems";

        public InventoryItemsController(IInventoryItemRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/inventoryitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync()
        {
            var results = await repository.GetInventoryItemListAsync();
            return Ok(results);
        }

        // api/inventoryitems/listing/1
        [Route("listing")]
        [HttpGet("listing/{mfrid:long}")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync(long mfrId)
        {
            var results = await repository.GetInventoryItemListAsync(mfrId);
            return Ok(results);
        }

        // api/inventoryitems/1/ABC123
        [HttpGet("{mfrid:long}/{partnumber}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long mfrId, string partNumber)
        {
            var result = await repository.GetInventoryItemAsync(mfrId, partNumber);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventoryitems/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long id)
        {
            var result = await repository.GetInventoryItemAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/inventoryitems/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryItemAsync(long id, InventoryItemToWrite itemDto)
        {
            var notFoundMessage = $"Could not find Inventory Item to update: {itemDto.PartNumber}";

            if (!await repository.InventoryItemExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var item = repository.GetInventoryItemEntityAsync(id).Result;

            // 2) Update domain entity with data in data transfer object(DTO)
            item.Manufacturer = itemDto.Manufacturer;
            item.ManufacturerId = itemDto.ManufacturerId;
            item.PartNumber = itemDto.PartNumber;
            item.Description = itemDto.Description;
            item.ProductCode = itemDto.ProductCode;
            item.ProductCodeId = itemDto.ProductCodeId;
            item.PartType = itemDto.PartType;
            item.QuantityOnHand = itemDto.QuantityOnHand;
            item.Cost = itemDto.Cost;
            item.SuggestedPrice = itemDto.SuggestedPrice;
            item.Labor = itemDto.Labor;

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            item.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            repository.UpdateInventoryItemAsync(item);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddInventoryItemAsync(InventoryItemToWrite itemCreateDto)
        {
            // 1. Convert dto to domain entity
            var item = new InventoryItem()
            {
                Manufacturer = itemCreateDto.Manufacturer,
                ManufacturerId = itemCreateDto.ManufacturerId,
                PartNumber = itemCreateDto.PartNumber,
                Description = itemCreateDto.Description,
                ProductCode = itemCreateDto.ProductCode,
                ProductCodeId = itemCreateDto.ProductCodeId,
                PartType = itemCreateDto.PartType,
                QuantityOnHand = itemCreateDto.QuantityOnHand,
                Cost = itemCreateDto.Cost,
                SuggestedPrice = itemCreateDto.SuggestedPrice,
                Labor = itemCreateDto.Labor
            };

            // 2. Add domain entity to repository
            await repository.AddInventoryItemAsync(item);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new mfrId & partNumber from database to consumer after save
            return Created(new Uri($"{BasePath}/{item.ManufacturerId}/{item.PartNumber}", UriKind.Relative), new { ManufacturerId = item.ManufacturerId, PartNumber = item.PartNumber });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryItemAsync(long id)
        {
            var itemFromRepository = await repository.GetInventoryItemAsync(id);
            if (itemFromRepository == null)
                return NotFound($"Could not find Inventory Item in the database to delete with Id = {id}.");

            await repository.DeleteInventoryItemAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
