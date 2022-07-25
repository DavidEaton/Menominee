//using CustomerVehicleManagement.Api.Configurations.Inventory;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase //ApplicationController  FIX ME - Route not found when inheriting from ApplicationController
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

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/inventoryitems/listing/1
        [Route("listing")]
        [HttpGet("listing/{manufacturerId:long}")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync(long manufacturerId)
        {
            var results = await itemRepository.GetItemsInListAsync(manufacturerId);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // api/inventoryitems/1/ABC123
        [HttpGet("{manufacturerId:long}/{partNumber}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long manufacturerId, string partNumber)
        {
            var result = await itemRepository.GetItemAsync(manufacturerId, partNumber);

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
            var item = await itemRepository.GetItemEntityAsync(id);

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
        public async Task<ActionResult<InventoryItemToRead>> AddInventoryItemAsync(InventoryItemToWrite itemToWrite)
        {
            InventoryItem item = InventoryItemHelper.ConvertWriteDtoToEntity(itemToWrite);

            //InventoryItem item = null;

            //if (itemToWrite.ItemType == InventoryItemType.Part)
            //    item = new(InventoryPartHelper.ConvertWriteDtoToEntity(itemToWrite.Part));
            //else if (itemToWrite.ItemType == InventoryItemType.Labor)
            //    item = new(InventoryLaborHelper.ConvertWriteDtoToEntity(itemToWrite.Labor));
            //else if (itemToWrite.ItemType == InventoryItemType.Tire)
            //    item = new(InventoryTireHelper.ConvertWriteDtoToEntity(itemToWrite.Tire));
            //else if (itemToWrite.ItemType == InventoryItemType.Package)
            //    item = new(InventoryPackageHelper.ConvertWriteDtoToEntity(itemToWrite.Package));
            //else if (itemToWrite.ItemType == InventoryItemType.Inspection)
            //    item = new(InventoryInspectionHelper.ConvertWriteDtoToEntity(itemToWrite.Inspection));
            //else if (itemToWrite.ItemType == InventoryItemType.Donation)
            //    item = new() { ItemType = InventoryItemType.Donation, Donation = new() };// (InventoryDonationHelper.ConvertWriteDtoToEntity(itemToWrite.Donation));
            //else if (itemToWrite.ItemType == InventoryItemType.GiftCertificate)
            //    item = new() { ItemType = InventoryItemType.GiftCertificate, GiftCertificate = new() };// (InventoryGiftCertificateHelper.ConvertWriteDtoToEntity(itemToWrite.GiftCertificate));
            //else if (itemToWrite.ItemType == InventoryItemType.Warranty)
            //    item = new(InventoryWarrantyHelper.ConvertWriteDtoToEntity(itemToWrite.Warranty));

            ////item.Manufacturer = ManufacturerHelper.ConvertToEntity(itemToWrite.Manufacturer);
            //item.ManufacturerId = itemToWrite.Manufacturer.Id;
            //item.ItemNumber = itemToWrite.ItemNumber;
            //item.Description = itemToWrite.Description;
            ////item.ProductCode = ProductCodeHelper.ConvertToEntity(itemToWrite.ProductCode);
            //item.ProductCodeId = itemToWrite.ProductCode.Id;

            await itemRepository.AddItemAsync(item);

            await itemRepository.SaveChangesAsync();

            return CreatedAtRoute("GetInventoryItemAsync",
                                  new { id = item.Id },
                                  InventoryItemHelper.ConvertEntityToReadDto(item));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryItemAsync(long id)
        {
            var notFoundMessage = $"Could not find Inventory Item in the database to delete with Id = {id}.";

            InventoryItem itemFromRepository = await itemRepository.GetItemEntityAsync(id);

            if (itemFromRepository == null)
                return NotFound(notFoundMessage);

            itemRepository.DeleteInventoryItem(itemFromRepository);
            await itemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
