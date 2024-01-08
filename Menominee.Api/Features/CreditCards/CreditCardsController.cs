using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using Menominee.Shared.Models.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.CreditCards
{
    public class CreditCardsController : BaseApplicationController<CreditCardsController>
    {
        private readonly ICreditCardRepository repository;

        public CreditCardsController(ICreditCardRepository repository, ILogger<CreditCardsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<CreditCardToReadInList>>> GetListAsync()
        {
            var results = await repository.GetListAsync();
            return Ok(results);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CreditCardToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(CreditCardToWrite creditCardFromCaller)
        {
            var creditCardFromRepository = await repository.GetEntityAsync(creditCardFromCaller.Id);

            if (creditCardFromRepository is null)
                return NotFound($"Could not find Credit Card to update: {creditCardFromCaller.Name}");

            if (creditCardFromRepository.Name != creditCardFromCaller.Name)
                creditCardFromRepository.SetName(creditCardFromCaller.Name);

            if (creditCardFromRepository.FeeType != creditCardFromCaller.FeeType)
                creditCardFromRepository.SetFeeType(creditCardFromCaller.FeeType);

            if (creditCardFromRepository.Fee != creditCardFromCaller.Fee)
                creditCardFromRepository.SetFee(creditCardFromCaller.Fee);

            if (creditCardFromRepository.IsAddedToDeposit != creditCardFromCaller.IsAddedToDeposit)
                creditCardFromRepository.SetIsAddedToDeposit(creditCardFromCaller.IsAddedToDeposit);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(CreditCardToWrite creditCardToAdd)
        {
            // No need to validate it here again, just call .Value right away
            var creditCard = CreditCard.Create(creditCardToAdd.Name, creditCardToAdd.FeeType, creditCardToAdd.Fee, creditCardToAdd.IsAddedToDeposit).Value;

            repository.Add(creditCard);
            await repository.SaveChangesAsync();

            return Created(new Uri($"api/creditCards/{creditCard.Id}",
                UriKind.Relative),
                new { creditCard.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            var cardFromRepository = await repository.GetEntityAsync(id);

            if (cardFromRepository is null)
                return NotFound($"Could not find Credit Card in the database to delete with Id: {id}.");

            repository.Delete(cardFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
