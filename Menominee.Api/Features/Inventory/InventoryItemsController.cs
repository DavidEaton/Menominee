using CSharpFunctionalExtensions;
using Menominee.Api.Common;
using Menominee.Api.Features.Manufacturers;
using Menominee.Api.Features.ProductCodes;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Inventory
{
    public class InventoryItemsController : BaseApplicationController<InventoryItemsController>
    {
        private readonly IInventoryItemRepository itemRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IProductCodeRepository productCodeRepository;

        public InventoryItemsController(IInventoryItemRepository itemRepository,
            IManufacturerRepository manufacturerRepository,
            IProductCodeRepository productCodeRepository,
            ILogger<InventoryItemsController> logger) : base(logger)
        {
            this.itemRepository =
                itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));

            this.manufacturerRepository =
                manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

            this.productCodeRepository =
                productCodeRepository ?? throw new ArgumentNullException(nameof(productCodeRepository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<InventoryItemToReadInList>>> GetListAsync([FromQuery] long? manufacturerId)
        {
            var result = manufacturerId.HasValue
                ? await itemRepository.GetListAsync(manufacturerId.Value)
                : await itemRepository.GetListAsync();

            return Ok(result);
        }

        [HttpGet("{manufacturerId:long}/itemNumber/{itemNumber}")]
        public async Task<ActionResult<InventoryItemToRead>> GetAsync(long manufacturerId, string itemNumber)
        {
            var result = await itemRepository.GetAsync(manufacturerId, itemNumber);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<InventoryItemToRead>> GetAsync(long id)
        {
            var result = await itemRepository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(long id, InventoryItemToWrite itemFromCaller)
        {
            var (itemFromRepository, manufacturerFromRepository, productCodeFromRepository) =
                await GetEntitiesForUpdate(itemFromCaller.Id, itemFromCaller);

            if (itemFromRepository == null || manufacturerFromRepository == null || productCodeFromRepository == null)
                return NotFound($"Could not find Inventory Item # {itemFromCaller.Id} to update.");

            var result = await UpdateInventoryItemProperties(itemFromRepository, itemFromCaller,
                manufacturerFromRepository, productCodeFromRepository);

            if (result.IsFailure)
                return BadRequest(result.Error);

            await itemRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(InventoryItemToWrite itemToAdd)
        {
            var failureMessage = $"Could not add new Inventory Item Number: {itemToAdd?.ItemNumber}.";

            var (manufacturerFromRepository, productCodeFromRepository, inventoryItems) =
                await GetEntitiesForAdd(itemToAdd);

            var partFromRepository = await itemRepository.GetPartEntityAsync(itemToAdd.Part.Id);

            if (manufacturerFromRepository == null || productCodeFromRepository == null/* || inventoryItems == null*/)
                return NotFound(
                    new ApiError
                    {
                        Message = failureMessage
                    });

            var inventoryItem = InventoryItemHelper.ConvertWriteDtoToEntity(itemToAdd,
                manufacturerFromRepository, productCodeFromRepository, partFromRepository, null);

            itemRepository.Add(inventoryItem);
            await itemRepository.SaveChangesAsync();

            return Created(new Uri($"api/InventoryItems/{inventoryItem.Id}",
                UriKind.Relative),
                new { inventoryItem.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var itemFromRepository = await itemRepository.GetEntityAsync(id);

            if (itemFromRepository is null)
                return NotFound($"Could not find Inventory Item in the database to delete with Id = {id}.");

            itemRepository.Delete(itemFromRepository);
            await itemRepository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<(InventoryItem itemFromRepository, Manufacturer manufacturerFromRepository, ProductCode productCodeFromRepository)> GetEntitiesForUpdate(long id, InventoryItemToWrite itemFromCaller)
        {
            var itemFromRepository = await itemRepository.GetEntityAsync(id);

            var manufacturerFromRepository = itemFromCaller.Manufacturer is not null
                ? await manufacturerRepository.GetEntityAsync(itemFromCaller.Manufacturer.Id)
                : null;

            var productCodeFromRepository = itemFromCaller.ProductCode is not null
                ? await productCodeRepository.GetEntityAsync(itemFromCaller.ProductCode.Id)
                : null;

            return (itemFromRepository, manufacturerFromRepository, productCodeFromRepository);
        }

        private async Task<(Manufacturer manufacturerFromRepository,
            ProductCode productCodeFromRepository,
            IReadOnlyList<InventoryItem> inventoryItems
            )> GetEntitiesForAdd(InventoryItemToWrite itemToAdd)
        {
            var manufacturerFromRepository =
                itemToAdd.Manufacturer is not null
                ? await manufacturerRepository.GetEntityAsync(itemToAdd.Manufacturer.Id)
                : null;

            var productCodeFromRepository =
                itemToAdd.ProductCode is not null
                ? await productCodeRepository.GetEntityAsync(itemToAdd.ProductCode.Id)
                : null;

            var inventoryItems = await itemRepository.GetEntitiesAsync(GetItemIds(itemToAdd));

            return (manufacturerFromRepository, productCodeFromRepository, inventoryItems);
        }

        private async Task<Result> UpdateInventoryItemProperties(InventoryItem itemFromRepository, InventoryItemToWrite itemFromCaller, Manufacturer manufacturerFromRepository, ProductCode productCodeFromRepository)
        {
            return itemFromRepository.UpdateProperties(
                itemFromCaller.ItemNumber,
                itemFromCaller.Description,
                manufacturerFromRepository,
                productCodeFromRepository,
                itemFromCaller.Part is not null
                    ? UpdateInventoryItemPart(itemFromCaller.Part).Result.Value
                    : null,
                itemFromCaller.Labor is not null
                    ? await itemRepository.GetLaborEntityAsync(itemFromCaller.Labor.Id)
                    : null,
                itemFromCaller.Tire is not null
                    ? await itemRepository.GetTireEntityAsync(itemFromCaller.Tire.Id)
                    : null,
                itemFromCaller.Package is not null
                    ? await itemRepository.GetPackageEntityAsync(itemFromCaller.Package.Id)
                    : null,
                itemFromCaller.Inspection is not null
                    ? await itemRepository.GetInspectionEntityAsync(itemFromCaller.Inspection.Id)
                    : null,
                itemFromCaller.Warranty is not null
                    ? await itemRepository.GetWarrantyEntityAsync(itemFromCaller.Warranty.Id)
                    : null);
        }

        private async Task<Result<InventoryItemPart>> UpdateInventoryItemPart(InventoryItemPartToWrite part)
        {
            var partFromRepository = await itemRepository.GetPartEntityAsync(part.Id);

            var results = new List<Result>
            {
                partFromRepository.SetList(part.List),
                partFromRepository.SetRetail(part.Retail),
                partFromRepository.SetCost(part.Cost),
                partFromRepository.SetCore(part.Core),
                partFromRepository.SetTechAmount(TechAmount.Create(part.TechAmount.PayType, part.TechAmount.Amount, part.TechAmount.SkillLevel).Value),
                partFromRepository.SetFractional(part.Fractional),
            };

            if (part.LineCode is not null)
                results.Add(partFromRepository.SetLineCode(part.LineCode));

            if (part.SubLineCode is not null)
                results.Add(partFromRepository.SetSubLineCode(part.SubLineCode));

            var exciseFeeResults = partFromRepository.ExciseFees.Select(fee =>
            {
                var feeToWrite = part.ExciseFees.FirstOrDefault(f => f.Id == fee.Id);
                if (feeToWrite is not null)
                {
                    var descriptionResult = fee.SetDescription(feeToWrite.Description);
                    var feeTypeResult = fee.SetFeeType(feeToWrite.FeeType);
                    var amountResult = fee.SetAmount(feeToWrite.Amount);

                    return Result.Combine(descriptionResult, feeTypeResult, amountResult);
                }

                return Result.Success();
            }).ToList();

            var updateExciseFeesResult = Result.Combine(exciseFeeResults);

            if (updateExciseFeesResult.IsFailure)
                return Result.Failure<InventoryItemPart>(updateExciseFeesResult.Error);

            if (results.Any(result => result.IsFailure))
            {
                var errorMessages = results.Where(result => result.IsFailure)
                                           .Select(result => result.Error)
                                           .Aggregate((message1, message2) => $"{message1}, {message2}");

                return Result.Failure<InventoryItemPart>(errorMessages);
            }

            return Result.Success(partFromRepository);
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
