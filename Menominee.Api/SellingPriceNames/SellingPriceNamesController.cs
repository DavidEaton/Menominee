using Menominee.Api.Common;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.SellingPriceNames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.SellingPriceNames;

public class SellingPriceNamesController : BaseApplicationController<SellingPriceNamesController>
{
    private readonly ISellingPriceNameRepository repository;
    private readonly string BasePath = "/api/sellingpricenames/";

    public SellingPriceNamesController(ISellingPriceNameRepository sellingPriceNameRepository, ILogger<SellingPriceNamesController> logger) : base(logger)
    {
        this.repository = sellingPriceNameRepository ??
            throw new ArgumentNullException(nameof(sellingPriceNameRepository));
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<SellingPriceNameToRead>>> GetSellingPriceNamesList()
    {
        var sellingPriceNames = await repository.GetAll();

        return sellingPriceNames.Count == 0
            ? Ok(new List<SellingPriceNameToRead>())
            : Ok(sellingPriceNames
                .Select(sellingPriceName => SellingPriceNameHelper.ConvertToReadDto(sellingPriceName))
                .ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SellingPriceNameToRead>> GetSellingPriceName(long id)
    {
        var sellingPriceName = await repository.GetEntity(id);

        return sellingPriceName is not null
            ? Ok(SellingPriceNameHelper.ConvertToReadDto(sellingPriceName))
            : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<SellingPriceNameToRead>> AddSellingPriceName(SellingPriceNameToWrite sellingPriceNameToAdd)
    {
        var sellingPriceName = SellingPriceName.Create(sellingPriceNameToAdd.Name).Value;

        repository.Add(sellingPriceName);
        await repository.SaveChanges();

        return Created(new Uri($"{BasePath}/{sellingPriceName.Id}", UriKind.Relative), new { id = sellingPriceName.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateSellingPriceName(long id, SellingPriceNameToWrite updatedSellingPriceName)
    {
        var sellingPriceNameFromRepository = await repository.GetEntity(id);

        if (sellingPriceNameFromRepository is null) return NotFound();

        var nameResult = sellingPriceNameFromRepository.SetName(updatedSellingPriceName.Name);

        if (nameResult.IsFailure) return BadRequest(nameResult.Error);

        await repository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteSellingPriceName(long id)
    {
        var sellingPriceNameFromRepository = await repository.GetEntity(id);

        if (sellingPriceNameFromRepository is null) return NotFound();

        repository.Delete(sellingPriceNameFromRepository);
        await repository.SaveChanges();

        return NoContent();
    }
}
