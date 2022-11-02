using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class MaintenanceItemsController : ApplicationController
    {
        private readonly IMaintenanceItemRepository maintenanceItemRepository;
        private readonly IInventoryItemRepository inventoryItemRepository;
        private readonly string BasePath = "/api/maintenanceitems";

        public MaintenanceItemsController(IMaintenanceItemRepository maintenanceItemRepository, IInventoryItemRepository inventoryItemRepository)
        {
            this.maintenanceItemRepository =
                maintenanceItemRepository ?? throw new ArgumentNullException(nameof(maintenanceItemRepository));

            this.inventoryItemRepository =
                inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
        }

        // api/maintenanceitems/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MaintenanceItemToReadInList>>> GetMaintenanceItemsListAsync()
        {
            var result = await maintenanceItemRepository.GetItemsInListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        // api/maintenanceitems/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<MaintenanceItemToRead>> GetMaintenanceItemAsync(long id)
        {
            var result = await maintenanceItemRepository.GetItemAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        // api/maintenanceitems/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateMaintenanceItemAsync(MaintenanceItemToWrite itemFromCaller, long id)
        {
            var notFoundMessage = $"Could not find Maintenance Item # {id} to update.";

            if (!await maintenanceItemRepository.ItemExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entities from repositories:
            MaintenanceItem itemFromRepository = await maintenanceItemRepository.GetItemEntityAsync(id);

            if (itemFromRepository is null)
                return NotFound(notFoundMessage);

            // 2) Update domain "aggregate root" entity (MaintenanceItem) with data in
            // data contract/transfer object(DTO).
            if (itemFromRepository.InventoryItem.Id != itemFromCaller.Item.Id)
            {
                var resultOrError = itemFromRepository.SetInventoryItem(
                    await inventoryItemRepository.GetItemEntityAsync(
                        itemFromCaller.Item.Id));

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            if (itemFromRepository.DisplayOrder != itemFromCaller.DisplayOrder)
            {
                var resultOrError = itemFromRepository.SetDisplayOrder(itemFromCaller.DisplayOrder);

                if (resultOrError.IsFailure)
                    return BadRequest(resultOrError.Error);
            }

            await maintenanceItemRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddMaintenanceItemAsync(
            MaintenanceItemToWrite itemToAdd)
        {
            InventoryItem inventoryItem = await inventoryItemRepository.GetItemEntityAsync(itemToAdd.Item.Id);

            if (inventoryItem is null)
                return NotFound($"Could not add new Inventory Item Number: {itemToAdd?.Item.ItemNumber}.");

            MaintenanceItem maintenanceItemEntity = MaintenanceItemHelper.ConvertWriteDtoToEntity(itemToAdd, inventoryItem);

            await maintenanceItemRepository.AddItemAsync(maintenanceItemEntity);

            await maintenanceItemRepository.SaveChangesAsync();

            return Created(
                new Uri($"{BasePath}/{maintenanceItemEntity.Id}",
                UriKind.Relative),
                new
                {
                    maintenanceItemEntity.Id
                });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteMaintenanceItemAsync(long id)
        {
            var notFoundMessage = $"Could not find Maintenance Item in the database to delete with Id = {id}.";

            MaintenanceItem itemFromRepository = await maintenanceItemRepository.GetItemEntityAsync(id);

            if (itemFromRepository == null)
                return NotFound(notFoundMessage);

            maintenanceItemRepository.DeleteItem(itemFromRepository);
            await maintenanceItemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}