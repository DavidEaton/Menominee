using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Manufacturers;
using CustomerVehicleManagement.Api.ProductCodes;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync()
        {
            var result = await itemRepository.GetItemsInListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [Route("listing")]
        [HttpGet("listing/{manufacturerId:long}")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetInventoryItemsListAsync(long manufacturerId)
        {
            var result = await itemRepository.GetItemsInListAsync(manufacturerId);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{manufacturerId:long}/{partNumber}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long manufacturerId, string partNumber)
        {
            var result = await itemRepository.GetItemAsync(manufacturerId, partNumber);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InventoryItemToRead>> GetInventoryItemAsync(long id)
        {
            var result = await itemRepository.GetItemAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateInventoryItemAsync(long id, InventoryItemToWrite itemFromCaller)
        {
            var notFoundMessage = $"Could not find Inventory Item # {id} to update.";

            //1) Get domain entities from repositories:
            var itemFromRepository =
                await itemRepository.GetItemEntityAsync(id);

            var manufacturerFromRepository =
                await manufacturerRepository.GetManufacturerEntityAsync(itemFromCaller.Manufacturer.Id);

            var productCodeFromRepository =
                await productCodeRepository.GetProductCodeEntityAsync(itemFromCaller.ProductCode.Id);

            if (itemFromRepository is null
                || manufacturerFromRepository is null
                || productCodeFromRepository is null)
                return NotFound(notFoundMessage);

            // 2) Update domain "aggregate root" entity (InventoryItem) with data in
            // data contract/transfer object(DTO).
            // Update each member of aggregate root InventoryItem, or return a Bad
            // Resuest response with error message, in case of Failure:
            if (itemFromRepository.Manufacturer.Id != itemFromCaller.Manufacturer.Id)
            {
                var resultOrError = itemFromRepository.SetManufacturer(manufacturerFromRepository);

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.ItemNumber != itemFromCaller.ItemNumber)
            {
                var resultOrError = itemFromRepository.SetItemNumber(itemFromCaller.ItemNumber);

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

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
            };

            if (itemFromRepository.Part.Id != itemFromCaller.Part.Id)
            {
                var resultOrError = itemFromRepository.SetPart(
                    await itemRepository.GetInventoryItemPartEntityAsync(itemFromCaller.Part.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.Labor.Id != itemFromCaller.Labor.Id)
            {
                var resultOrError = itemFromRepository.SetLabor(
                    await itemRepository.GetInventoryItemLaborEntityAsync(itemFromCaller.Labor.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.Tire.Id != itemFromCaller.Tire.Id)
            {
                var resultOrError = itemFromRepository.SetTire(
                    await itemRepository.GetInventoryItemTireEntityAsync(itemFromCaller.Tire.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.Package.Id != itemFromCaller.Package.Id)
            {
                var resultOrError = itemFromRepository.SetPackage(
                    await itemRepository.GetInventoryItemPackageEntityAsync(itemFromCaller.Package.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.Inspection.Id != itemFromCaller.Inspection.Id)
            {
                var resultOrError = itemFromRepository.SetInspection(
                    await itemRepository.GetInventoryItemInspectionEntityAsync(itemFromCaller.Inspection.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            if (itemFromRepository.Warranty.Id != itemFromCaller.Warranty.Id)
            {
                var resultOrError = itemFromRepository.SetWarranty(
                    await itemRepository.GetInventoryItemWarrantyEntityAsync(itemFromCaller.Warranty.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            };

            await itemRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryItemAsync(InventoryItemToWrite itemToAdd)
        {
            var failureMessage = $"Could not add new Inventory Item Number: {itemToAdd?.ItemNumber}.";

            var manufacturer =
               await manufacturerRepository.GetManufacturerEntityAsync(itemToAdd.Manufacturer.Id);

            var productCode =
                await productCodeRepository.GetProductCodeEntityAsync(itemToAdd.ProductCode.Id);

            var inventoryItems =
                await itemRepository.GetInventoryItemEntitiesAsync(GetItemIds(itemToAdd));

            if (manufacturer is null
                || productCode is null
                || inventoryItems is null)
                return NotFound(failureMessage);

            InventoryItem inventoryItemEntity = InventoryItemHelper.ConvertWriteDtoToEntity(
                itemToAdd,
                manufacturer,
                productCode,
                inventoryItems);

            await itemRepository.AddItemAsync(inventoryItemEntity);
            await itemRepository.SaveChangesAsync();

            return Created(
                new Uri($"{BasePath}/{inventoryItemEntity.Id}",
                UriKind.Relative),
                new
                {
                    inventoryItemEntity.Id
                });
        }

        private static List<long> GetItemIds(InventoryItemToWrite itemToAdd)
        {
            List<long> ids = new();

            if (itemToAdd.Part is not null)
                ids.Add(itemToAdd.Part.Id);

            if (itemToAdd.Labor is not null)
                ids.Add(itemToAdd.Labor.Id);

            if (itemToAdd.Tire is not null)
                ids.Add(itemToAdd.Tire.Id);

            if (itemToAdd.Package is not null)
                foreach (var item in itemToAdd.Package.Items)
                    ids.Add(item.Id);

            if (itemToAdd.Inspection is not null)
                ids.Add(itemToAdd.Inspection.Id);

            if (itemToAdd.Warranty is not null)
                ids.Add(itemToAdd.Warranty.Id);

            return ids;
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteInventoryItemAsync(long id)
        {
            var notFoundMessage = $"Could not find Inventory Item in the database to delete with Id = {id}.";

            InventoryItem itemFromRepository = await itemRepository.GetItemEntityAsync(id);

            if (itemFromRepository is null)
                return NotFound(notFoundMessage);

            itemRepository.DeleteInventoryItem(itemFromRepository);
            await itemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
