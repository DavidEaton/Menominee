//using CustomerVehicleManagement.Api.Configurations.Inventory;
using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.ProductCodes;
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
    public class InventoryItemsController : ControllerBase //ApplicationController  FIX ME - Route not found when inheriting from ApplicationController
    {
        private readonly IInventoryItemRepository itemRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IProductCodeRepository productCodeRepository;

        public InventoryItemsController(IInventoryItemRepository itemRepository, IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            this.manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
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
        public async Task<IActionResult> UpdateInventoryItemAsync(long id, InventoryItemToWrite itemFromCaller)
        {
            var notFoundMessage = $"Could not find Inventory Item # {id} to update.";

            if (!await itemRepository.ItemExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entities from repositories
            var itemFromRepository = await itemRepository.GetItemEntityAsync(id);

            var manufacturerFromRepository =
                await manufacturerRepository.GetManufacturerEntityAsync(itemFromCaller.Manufacturer.Id);
            var productCodeFromRepository =
                await productCodeRepository.GetProductCodeEntityAsync(itemFromCaller.ProductCode.Id);

            // 2) Update domain aggregate entity (InventoryItem) with data in
            // data transfer object(DTO).
            // Update each member of InventoryItem, or return a Bad Resuest response
            // with error message, in case of Failure.
            if (itemFromRepository.Manufacturer.Id != itemFromCaller.Manufacturer.Id)
            {
                var resultOrError = itemFromRepository.SetManufacturer(manufacturerFromRepository);
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.ItemNumber != itemFromCaller.ItemNumber)
            {
                var resultOrError = itemFromRepository.SetItemNumber(itemFromCaller.ItemNumber);
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Description != itemFromCaller.Description)
            {
                var resultOrError = itemFromRepository.SetDescription(itemFromCaller.Description);
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.ProductCode.Id != itemFromCaller.ProductCode.Id)
            {
                var resultOrError = itemFromRepository.SetProductCode(productCodeFromRepository);
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.ItemType != itemFromCaller.ItemType)
            {
                var resultOrError = itemFromRepository.SetItemType(itemFromCaller.ItemType);
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Part.Id != itemFromCaller.Part.Id)
            {
                var resultOrError = itemFromRepository.SetPart(await itemRepository.GetInventoryItemPartEntityAsync(
                    itemFromCaller.Part.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Tire.Id != itemFromCaller.Tire.Id)
            {
                var resultOrError = itemFromRepository.SetTire(await itemRepository.GetInventoryItemTireEntityAsync(
                    itemFromCaller.Tire.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Inspection.Id != itemFromCaller.Inspection.Id)
            {
                var resultOrError = itemFromRepository.SetInspection(
                    await itemRepository.GetInventoryItemInspectionEntityAsync(
                    itemFromCaller.Inspection.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Labor.Id != itemFromCaller.Labor.Id)
            {
                var resultOrError = itemFromRepository.SetLabor(
                    await itemRepository.GetInventoryItemLaborEntityAsync(
                    itemFromCaller.Labor.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Package.Id != itemFromCaller.Package.Id)
            {
                var resultOrError = itemFromRepository.SetPackage(
                    await itemRepository.GetInventoryItemPackageEntityAsync(
                    itemFromCaller.Package.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.Warranty.Id != itemFromCaller.Warranty.Id)
            {
                var resultOrError = itemFromRepository.SetWarranty(await itemRepository.GetInventoryItemWarrantyEntityAsync(
                    itemFromCaller.Warranty.Id));
                
                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

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
