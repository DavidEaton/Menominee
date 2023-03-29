using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.ProductCodes;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
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
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IProductCodeRepository productCodeRepository;
        private readonly string BasePath = "/api/inventoryitems";

        public InventoryItemsController(IInventoryItemRepository itemRepository,
            IManufacturerRepository manufacturerRepository,
            IProductCodeRepository productCodeRepository)
        {
            this.itemRepository =
                itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));

            this.manufacturerRepository =
                manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

            this.productCodeRepository =
                productCodeRepository ?? throw new ArgumentNullException(nameof(productCodeRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetItemsInList([FromQuery] long? manufacturerId)
        {
            var result = manufacturerId.HasValue
                ? await itemRepository.GetItemsInList(manufacturerId.Value)
                : await itemRepository.GetItemsInList();

            return result.Count > 0
                ? Ok(result)
                : NotFound();
        }

        [HttpGet("{manufacturerId:long}/partnumber/{partNumber}")]
        public async Task<ActionResult<InventoryItemToRead>> Get(long manufacturerId, string itemNumber)
        {
            var result = await itemRepository.GetItem(manufacturerId, itemNumber);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InventoryItemToRead>> Get(long id)
        {
            var result = await itemRepository.GetItem(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, InventoryItemToWrite itemFromCaller)
        {
            var notFoundMessage = $"Could not find Inventory Item # {id} to update.";

            var (itemFromRepository, manufacturerFromRepository, productCodeFromRepository) =
                await GetEntitiesForUpdate(id, itemFromCaller);

            if (itemFromRepository == null || manufacturerFromRepository == null || productCodeFromRepository == null)
                return NotFound(notFoundMessage);

            var result = await UpdateInventoryItemPropertiesAsync(itemFromRepository, itemFromCaller,
                manufacturerFromRepository, productCodeFromRepository);

            if (result.IsFailure)
                return BadRequest(result.Error);

            await itemRepository.SaveChanges();

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Add(InventoryItemToWrite itemToAdd)
        {
            var failureMessage = $"Could not add new Inventory Item Number: {itemToAdd?.ItemNumber}.";

            // Get domain entities from repositories
            var (manufacturerFromRepository, productCodeFromRepository, inventoryItems) =
                await GetEntitiesForAdd(itemToAdd);
            if (manufacturerFromRepository == null || productCodeFromRepository == null || inventoryItems == null)
                return NotFound(failureMessage);

            // Convert and add the new inventory item
            var inventoryItemEntity = InventoryItemHelper.ConvertWriteDtoToEntity(itemToAdd,
                manufacturerFromRepository, productCodeFromRepository, inventoryItems);

            await itemRepository.Add(inventoryItemEntity);
            await itemRepository.SaveChanges();

            return Created(
                new Uri($"{BasePath}/{inventoryItemEntity.Id}", UriKind.Relative),
                new { inventoryItemEntity.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var notFoundMessage = $"Could not find Inventory Item in the database to delete with Id = {id}.";

            var itemFromRepository = await itemRepository.GetItemEntity(id);

            if (itemFromRepository is null)
                return NotFound(notFoundMessage);

            itemRepository.Delete(itemFromRepository);

            await itemRepository.SaveChanges();

            return NoContent();
        }

        private async Task<(InventoryItem itemFromRepository, Manufacturer manufacturerFromRepository, ProductCode productCodeFromRepository)> GetEntitiesForUpdate(long id, InventoryItemToWrite itemFromCaller)
        {
            var itemFromRepository = await itemRepository.GetItemEntity(id);

            var manufacturerFromRepository = itemFromCaller.Manufacturer is not null
                ? await manufacturerRepository.GetManufacturerEntityAsync(itemFromCaller.Manufacturer.Id)
                : null;

            var productCodeFromRepository = itemFromCaller.ProductCode is not null
                ? await productCodeRepository.GetProductCodeEntityAsync(itemFromCaller.ProductCode.Id)
                : null;

            return (itemFromRepository, manufacturerFromRepository, productCodeFromRepository);
        }

        private async Task<(Manufacturer manufacturerFromRepository, ProductCode productCodeFromRepository, IReadOnlyList<InventoryItem> inventoryItems)> GetEntitiesForAdd(InventoryItemToWrite itemToAdd)
        {
            var manufacturerFromRepository = itemToAdd.Manufacturer is not null ?
                await manufacturerRepository.GetManufacturerEntityAsync(itemToAdd.Manufacturer.Id)
                : null;

            var productCodeFromRepository = itemToAdd.ProductCode is not null
                ? await productCodeRepository.GetProductCodeEntityAsync(itemToAdd.ProductCode.Id)
                : null;

            var inventoryItems = await itemRepository.GetInventoryItemEntities(GetItemIds(itemToAdd));

            return (manufacturerFromRepository, productCodeFromRepository, inventoryItems);
        }

        private async Task<Result> UpdateInventoryItemPropertiesAsync(InventoryItem itemFromRepository, InventoryItemToWrite itemFromCaller, Manufacturer manufacturerFromRepository, ProductCode productCodeFromRepository)
        {
            return itemFromRepository.UpdateProperties(
                itemFromCaller.ItemNumber,
                itemFromCaller.Description,
                manufacturerFromRepository,
                productCodeFromRepository,
                itemFromCaller.Part is not null
                    ? await itemRepository.GetInventoryItemPartEntity(itemFromCaller.Part.Id)
                    : null,
                itemFromCaller.Labor is not null
                    ? await itemRepository.GetInventoryItemLaborEntity(itemFromCaller.Labor.Id)
                    : null,
                itemFromCaller.Tire is not null
                    ? await itemRepository.GetInventoryItemTireEntity(itemFromCaller.Tire.Id)
                    : null,
                itemFromCaller.Package is not null
                    ? await itemRepository.GetInventoryItemPackageEntity(itemFromCaller.Package.Id)
                    : null,
                itemFromCaller.Inspection is not null
                    ? await itemRepository.GetInventoryItemInspectionEntity(itemFromCaller.Inspection.Id)
                    : null,
                itemFromCaller.Warranty is not null
                    ? await itemRepository.GetInventoryItemWarrantyEntity(itemFromCaller.Warranty.Id)
                    : null);
        }

        private static List<long> GetItemIds(InventoryItemToWrite itemToAdd)
        {
            return new List<long?>
        {
            itemToAdd.Part?.Id,
            itemToAdd.Labor?.Id,
            itemToAdd.Tire?.Id,
            itemToAdd.Inspection?.Id,
            itemToAdd.Warranty?.Id
        }
            .Concat(itemToAdd.Package?.Items?.Select(item => (long?)item.Id) ?? Enumerable.Empty<long?>())
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .ToList();
        }
    }
}
