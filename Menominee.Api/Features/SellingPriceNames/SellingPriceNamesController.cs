using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.SellingPriceNames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.SellingPriceNames;

public class SellingPriceNamesController : BaseApplicationController<SellingPriceNamesController>
{
    private readonly ISellingPriceNameRepository repository;

    public SellingPriceNamesController(ISellingPriceNameRepository sellingPriceNameRepository, ILogger<SellingPriceNamesController> logger) : base(logger)
    {
        repository = sellingPriceNameRepository ??
            throw new ArgumentNullException(nameof(sellingPriceNameRepository));
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<SellingPriceNameToRead>>> GetListAsync()
    {
        var sellingPriceNames = await repository.GetAllAsync();

        return sellingPriceNames.Count == 0
            ? Ok(new List<SellingPriceNameToRead>())
            : Ok(sellingPriceNames
                .Select(sellingPriceName => SellingPriceNameHelper.ConvertToReadDto(sellingPriceName))
                .ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SellingPriceNameToRead>> GetAsync(long id)
    {
        var sellingPriceName = await repository.GetEntityAsync(id);

        return sellingPriceName is not null
            ? Ok(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName))
            : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PostResponse>> AddAsync(SellingPriceNameToWrite sellingPriceNameToAdd)
    {
        // No need to validate it here again, just call .Value right away
        var sellingPriceName = SellingPriceName.Create(sellingPriceNameToAdd.Name).Value;

        repository.Add(sellingPriceName);
        await repository.SaveChangesAsync();

        return Created(new Uri($"/api/SellingPriceNamesController/{sellingPriceName.Id}",
            UriKind.Relative),
            new { sellingPriceName.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateAsync(SellingPriceNameToWrite updatedSellingPriceName)
    {
        var sellingPriceNameFromRepository = await repository.GetEntityAsync(updatedSellingPriceName.Id);

        if (sellingPriceNameFromRepository is null)
            return NotFound($"Could not find Selling Price Name in the database to delete with Id: {updatedSellingPriceName.Id}.");

        var nameResult = sellingPriceNameFromRepository.SetName(updatedSellingPriceName.Name);

        if (nameResult.IsFailure) return BadRequest(nameResult.Error);

        await repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        var sellingPriceNameFromRepository = await repository.GetEntityAsync(id);

        if (sellingPriceNameFromRepository is null) return NotFound();

        repository.Delete(sellingPriceNameFromRepository);
        await repository.SaveChangesAsync();

        return NoContent();
    }
}
