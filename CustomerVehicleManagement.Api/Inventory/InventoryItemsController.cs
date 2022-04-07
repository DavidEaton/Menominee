//using CustomerVehicleManagement.Api.Configurations.Inventory;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Helpers.Inventory;
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
        private readonly IInventoryItemRepository itemRepository;
        private readonly InventoryPartsController partsController;
        private readonly InventoryLaborController laborController;
        private readonly InventoryTiresController tiresController;
        //private readonly string BasePath = "/api/inventoryitems";

        public InventoryItemsController(IInventoryItemRepository itemRepository,
                                        InventoryPartsController partsController,
                                        InventoryLaborController laborController,
                                        InventoryTiresController tiresController)
        {
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            this.partsController = partsController ?? throw new ArgumentNullException(nameof(partsController));
            this.laborController = laborController ?? throw new ArgumentNullException(nameof(laborController));
            this.tiresController = tiresController ?? throw new ArgumentNullException(nameof(tiresController));
        }

        // api/inventoryitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync()
        {
            var results = await itemRepository.GetItemsAsync();
            return Ok(results);
        }

        //// api/inventoryitems/listing/1
        //[Route("listing")]
        //[HttpGet("listing/{mfrid:long}")]
        //public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync(long mfrId)
        //{
        //    var results = await repository.GetItemsAsync(mfrId);
        //    return Ok(results);
        //}

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
            InventoryItemToRead itemFromRepository = await itemRepository.GetItemAsync(id);

            if (itemFromRepository == null || itemFromRepository?.ItemType == null)
                return NotFound($"Could not find Inventory Item in the database to update.");

            if (itemFromRepository.ItemType == InventoryItemType.Part)
                await partsController.UpdateInventoryPartAsync(itemFromRepository.Part.Id, itemToWrite.Part);
            else if (itemFromRepository.ItemType == InventoryItemType.Labor)
                await laborController.UpdateInventoryLaborAsync(itemFromRepository.Labor.Id, itemToWrite.Labor);
            else if (itemFromRepository.ItemType == InventoryItemType.Tire)
                await tiresController.UpdateInventoryTireAsync(itemFromRepository.Tire.Id, itemToWrite.Tire);

            itemRepository.FixTrackingState();

            if (await itemRepository.SaveChangesAsync())
                return NoContent();

            return BadRequest($"Failed to update .");
        }

        // api/inventoryitems/1
        //[HttpPut("{id:long}")]
        //public async Task<IActionResult> UpdateInventoryItemAsync(long id, InventoryItemToWrite itemDto)
        //{
        //    var notFoundMessage = $"Could not find Inventory Item to update: {itemDto.ItemNumber}";

        //    if (!await repository.InventoryItemExistsAsync(id))
        //        return NotFound(notFoundMessage);

        //    //1) Get domain entity from repository
        //    var item = repository.GetInventoryItemEntityAsync(id).Result;

        //    // 2) Update domain entity with data in data transfer object(DTO)
        //    item.Manufacturer = itemDto.Manufacturer;
        //    item.ManufacturerId = itemDto.ManufacturerId;
        //    item.ItemNumber = itemDto.ItemNumber;
        //    item.Description = itemDto.Description;
        //    item.ProductCode = itemDto.ProductCode;
        //    item.ProductCodeId = itemDto.ProductCodeId;
        //    item.ItemType = itemDto.ItemType;


        //    // Update the objects ObjectState and sych the EF Change Tracker
        //    // 3) Set entity's TrackingState to Modified
        //    item.SetTrackingState(TrackingState.Modified);

        //    // 4) FixTrackingState: moves entity state tracking into the context
        //    repository.FixTrackingState();

        //    repository.UpdateInventoryItemAsync(item);

        //    await repository.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpPost]
        public async Task<ActionResult> AddInventoryItemAsync(InventoryItemToWrite itemToAdd)
        {
            InventoryItem item = null;

            if (itemToAdd.ItemType == InventoryItemType.Part)
                item = new(InventoryPartHelper.CreateEntityFromWriteDto(itemToAdd.Part));
            else if (itemToAdd.ItemType == InventoryItemType.Labor)
                item = new(InventoryLaborHelper.CreateEntityFromWriteDto(itemToAdd.Labor));
            else if (itemToAdd.ItemType == InventoryItemType.Tire)
                item = new(InventoryTireHelper.CreateEntityFromWriteDto(itemToAdd.Tire));

            item.ManufacturerId = itemToAdd.ManufacturerId;
            item.ItemNumber = itemToAdd.ItemNumber;
            item.Description = itemToAdd.Description;
            item.ProductCodeId = itemToAdd.ProductCodeId;

            await itemRepository.AddItemAsync(item);

            if (!await itemRepository.SaveChangesAsync())
                return BadRequest($"Failed to add {itemToAdd}.");

            InventoryItemToRead itemFromRepository = await itemRepository.GetItemAsync(item.Id);

            if (itemFromRepository == null)
                return BadRequest($"Failed to add {itemToAdd}.");

            return CreatedAtRoute("GetInventoryItemAsync",
                                  new { id = itemFromRepository.Id },
                                  itemFromRepository);
            //return Created(new Uri($"{BasePath}/{item.ManufacturerId}/{item.ItemNumber}", UriKind.Relative), new { ManufacturerId = item.ManufacturerId, ItemNumber = item.ItemNumber });
        }

        //[HttpPost]
        //public async Task<ActionResult> AddInventoryItemAsync(InventoryItemToWrite itemCreateDto)
        //{
        //    // 1. Convert dto to domain entity
        //    var item = new InventoryItem()
        //    {
        //        Manufacturer = itemCreateDto.Manufacturer,
        //        ManufacturerId = itemCreateDto.ManufacturerId,
        //        ItemNumber = itemCreateDto.ItemNumber,
        //        Description = itemCreateDto.Description,
        //        ProductCode = itemCreateDto.ProductCode,
        //        ProductCodeId = itemCreateDto.ProductCodeId,
        //        ItemType = itemCreateDto.ItemType
        //        //QuantityOnHand = itemCreateDto.QuantityOnHand,
        //        //Cost = itemCreateDto.Cost,
        //        //SuggestedPrice = itemCreateDto.SuggestedPrice,
        //        //Labor = itemCreateDto.Labor
        //    };

        //    // 2. Add domain entity to repository
        //    await repository.AddInventoryItemAsync(item);

        //    // 3. Save changes on repository
        //    await repository.SaveChangesAsync();

        //    // 4. Return new mfrId & itemNumber from database to consumer after save
        //    return Created(new Uri($"{BasePath}/{item.ManufacturerId}/{item.ItemNumber}", UriKind.Relative), new { ManufacturerId = item.ManufacturerId, ItemNumber = item.ItemNumber });
        //}

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
