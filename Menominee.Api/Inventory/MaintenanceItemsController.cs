using Menominee.Api.Common;
using Menominee.Api.Manufacturers;
using Menominee.Api.ProductCodes;
using Menominee.Common.Http;
using Menominee.Domain.Entities.Inventory;
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
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IProductCodeRepository productCodeRepository;
        public MaintenanceItemsController(
            IMaintenanceItemRepository maintenanceItemRepository,
            IInventoryItemRepository inventoryItemRepository,
            IManufacturerRepository manufacturerRepository,
            ILogger<MaintenanceItemsController> logger) : base(logger)
        {
            this.maintenanceItemRepository =
                maintenanceItemRepository ?? throw new ArgumentNullException(nameof(maintenanceItemRepository));

            this.inventoryItemRepository =
                inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));

            this.manufacturerRepository =
                manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<MaintenanceItemToReadInList>>> GetListAsync()
        {
            var results = await maintenanceItemRepository.GetListAsync();
            return Ok(results);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MaintenanceItemToRead>> GetAsync(long id)
        {
            var result = await maintenanceItemRepository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(MaintenanceItemToWrite itemFromCaller)
        {
            var itemFromRepository = await maintenanceItemRepository.GetEntityAsync(itemFromCaller.Id);
            if (itemFromRepository is null)
                return NotFound($"Could not find Maintenance Item # {itemFromCaller.Id} to update.");

            if (itemFromRepository.InventoryItem.Id != itemFromCaller.Item.Id)
                itemFromRepository.SetInventoryItem(
                    await inventoryItemRepository.GetEntityAsync(
                        itemFromCaller.Item.Id));

            if (itemFromRepository.DisplayOrder != itemFromCaller.DisplayOrder)
                itemFromRepository.SetDisplayOrder(itemFromCaller.DisplayOrder);

            await maintenanceItemRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(MaintenanceItemToWrite maintenanceItemToAdd)
        {
            if (await manufacturerRepository.GetEntityAsync(maintenanceItemToAdd.Item.Manufacturer.Id) is not Manufacturer manufacturer)
                return NotFound($"Could not find Manufacturer with Id {maintenanceItemToAdd.Item.Manufacturer.Id}");

            if (await productCodeRepository.GetEntityAsync(maintenanceItemToAdd.Item.ProductCode.Id) is not ProductCode productCode)
                return NotFound($"Could not find Product Code with Id {maintenanceItemToAdd.Item.ProductCode.Id}");

            InventoryItem item = null;
            InventoryItemPart part = null;

            if (maintenanceItemToAdd.Item.Part is not null)
            {
                part = await inventoryItemRepository.GetPartEntityAsync(maintenanceItemToAdd.Item.Part.Id);
                if (part is null)
                    return NotFound($"Could not find Part with Id {maintenanceItemToAdd.Item.Part.Id}.");
            }

            if (maintenanceItemToAdd.Item.Id == 0)
            {
                item = InventoryItem.Create(
                        manufacturer,
                        maintenanceItemToAdd.Item.ItemNumber,
                        maintenanceItemToAdd.Item.Description,
                        productCode,
                        maintenanceItemToAdd.Item.ItemType,
                        part)
                    .Value;
            }

            if (item is not null)
            {
                inventoryItemRepository.Add(item);
                await inventoryItemRepository.SaveChangesAsync();
            }

            if (maintenanceItemToAdd.Item.Id > 0)
            {
                var inventoryItemFromRepository = await inventoryItemRepository.GetEntityAsync(maintenanceItemToAdd.Item.Id);

                if (inventoryItemFromRepository is null)
                    return NotFound($"Could not add new Maintenance Item Number: {maintenanceItemToAdd?.Item.ItemNumber}.");

                item = inventoryItemFromRepository;
            }

            var maintenanceItem = MaintenanceItem.Create(
                maintenanceItemToAdd.DisplayOrder,
                item)
                    .Value;

            maintenanceItemRepository.Add(maintenanceItem);
            await maintenanceItemRepository.SaveChangesAsync();

            return Created(new Uri($"api/MaintenanceItems/{maintenanceItem.Id}",
                UriKind.Relative),
                new { maintenanceItem.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var itemFromRepository = await maintenanceItemRepository.GetEntityAsync(id);

            if (itemFromRepository == null)
                return NotFound($"Could not find Maintenance Item in the database to delete with Id = {id}.");

            maintenanceItemRepository.Delete(itemFromRepository);
            await maintenanceItemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}