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

            //1) Get domain entity from repository
            var creditCardFromRepository = await repository.GetCreditCardEntityAsync(id);

            // 2) Update domain entity with data in data transfer object(DTO)
            if (creditCardFromRepository.Name != creditCard.Name)
                creditCardFromRepository.SetName(creditCard.Name);

            if (creditCardFromRepository.FeeType != creditCard.FeeType)
                creditCardFromRepository.SetFeeType(creditCard.FeeType);

            if (creditCardFromRepository.Fee != creditCard.Fee)
                creditCardFromRepository.SetFee(creditCard.Fee);

            if (creditCardFromRepository.IsAddedToDeposit != creditCard.IsAddedToDeposit)
                creditCardFromRepository.SetIsAddedToDeposit(creditCard.IsAddedToDeposit);

            //cc.Processor = ccDto.Processor;

            await repository.UpdateCreditCardAsync(creditCardFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CreditCardToRead>> AddCreditCardAsync(CreditCardToWrite creditCardToAdd)
        {
            // 1. Convert dto to domain entity
            var creditCardOrError = CreditCard.Create(creditCardToAdd.Name, creditCardToAdd.FeeType, creditCardToAdd.Fee, creditCardToAdd.IsAddedToDeposit);

            if (creditCardOrError.IsFailure)
                return NotFound($"Could not add new Credit Card '{creditCardToAdd.Name}'.");

            CreditCard creditCard = creditCardOrError.Value;

            // 2. Add domain entity to repository
            await repository.AddCreditCardAsync(creditCard);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4.Return new Id from database to consumer after save
            return Created(
              new Uri($"api/creditCardscontroller/{creditCard.Id}", UriKind.Relative),
              new { creditCard.Id });
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
