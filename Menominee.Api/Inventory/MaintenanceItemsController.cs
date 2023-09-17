using Menominee.Api.Common;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
{
    public class MaintenanceItemsController : BaseApplicationController<MaintenanceItemsController>
    {
        private readonly IMaintenanceItemRepository maintenanceItemRepository;
        private readonly IInventoryItemRepository inventoryItemRepository;

        public MaintenanceItemsController(
            IMaintenanceItemRepository maintenanceItemRepository,
            IInventoryItemRepository inventoryItemRepository,
            ILogger<MaintenanceItemsController> logger) : base(logger)
        {
            this.maintenanceItemRepository =
                maintenanceItemRepository ?? throw new ArgumentNullException(nameof(maintenanceItemRepository));

            this.inventoryItemRepository =
                inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
        }

        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MaintenanceItemToReadInList>>> GetListAsync()
        {
            var result = await maintenanceItemRepository.GetItemsInListAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MaintenanceItemToRead>> GetAsync(long id)
        {
            var result = await maintenanceItemRepository.GetItemAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(MaintenanceItemToWrite itemFromCaller, long id)
        {
            var notFoundMessage = $"Could not find Maintenance Item # {id} to update.";

            if (!await maintenanceItemRepository.ItemExistsAsync(id))
                return NotFound(notFoundMessage);

            var itemFromRepository = await maintenanceItemRepository.GetItemEntityAsync(id);

            if (itemFromRepository is null)
                return NotFound(notFoundMessage);

            if (itemFromRepository.InventoryItem.Id != itemFromCaller.Item.Id)
            {
                var resultOrError = itemFromRepository.SetInventoryItem(
                    await inventoryItemRepository.GetItemEntity(
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
        public async Task<ActionResult> AddAsync(
            MaintenanceItemToWrite itemToAdd)
        {
            var inventoryItem = await inventoryItemRepository.GetItemEntity(itemToAdd.Item.Id);

            if (inventoryItem is null)
                return NotFound($"Could not add new Inventory Item Number: {itemToAdd?.Item.ItemNumber}.");

            var maintenanceItemEntity = MaintenanceItemHelper.ConvertWriteDtoToEntity(itemToAdd, inventoryItem);

            await maintenanceItemRepository.AddItemAsync(maintenanceItemEntity);

            await maintenanceItemRepository.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAsync),
                new { id = maintenanceItemEntity.Id },
                    new { maintenanceItemEntity.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var notFoundMessage = $"Could not find Maintenance Item in the database to delete with Id = {id}.";

            var itemFromRepository = await maintenanceItemRepository.GetItemEntityAsync(id);

            if (itemFromRepository == null)
                return NotFound(notFoundMessage);

            maintenanceItemRepository.DeleteItem(itemFromRepository);
            await maintenanceItemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}