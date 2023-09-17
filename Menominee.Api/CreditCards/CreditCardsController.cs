using Menominee.Api.Common;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.CreditCards
{
    public class CreditCardsController : BaseApplicationController<CreditCardsController>
    {
        private readonly ICreditCardRepository repository;

        public CreditCardsController(ICreditCardRepository repository, ILogger<CreditCardsController> logger) : base(logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("listing")]
        public async Task<ActionResult<IReadOnlyList<CreditCardToReadInList>>> GetCreditCardListAsync()
        {
            var results = await repository.GetCreditCardListAsync();
            return Ok(results);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CreditCardToRead>> GetCreditCardAsync(long id)
        {
            var result = await repository.GetCreditCardAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateCreditCardAsync(long id, CreditCardToWrite creditCard)
        {
            if (!await repository.CreditCardExistsAsync(creditCard.Id))
                return NotFound($"Could not find Credit Card to update: {creditCard.Name}");

            var creditCardFromRepository = await repository.GetCreditCardEntityAsync(id);

            if (creditCardFromRepository.Name != creditCard.Name)
                creditCardFromRepository.SetName(creditCard.Name);

            if (creditCardFromRepository.FeeType != creditCard.FeeType)
                creditCardFromRepository.SetFeeType(creditCard.FeeType);

            if (creditCardFromRepository.Fee != creditCard.Fee)
                creditCardFromRepository.SetFee(creditCard.Fee);

            if (creditCardFromRepository.IsAddedToDeposit != creditCard.IsAddedToDeposit)
                creditCardFromRepository.SetIsAddedToDeposit(creditCard.IsAddedToDeposit);

            await repository.UpdateCreditCardAsync(creditCardFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CreditCardToRead>> AddCreditCardAsync(CreditCardToWrite creditCardToAdd)
        {
            var result = CreditCard.Create(creditCardToAdd.Name, creditCardToAdd.FeeType, creditCardToAdd.Fee, creditCardToAdd.IsAddedToDeposit);

            if (result.IsFailure)
                return BadRequest($"Could not add new Credit Card '{creditCardToAdd.Name}'.");

            await repository.AddCreditCardAsync(result.Value);

            await repository.SaveChangesAsync();

            return Created(
              new Uri($"api/creditCardscontroller/{result.Value.Id}", UriKind.Relative),
              new { result.Value.Id });
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteCreditCardAsync(long id)
        {
            var ccFromRepository = await repository.GetCreditCardAsync(id);
            if (ccFromRepository == null)
                return NotFound($"Could not find Credit Card in the database to delete with Id: {id}.");

            await repository.DeleteCreditCardAsync(id);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
