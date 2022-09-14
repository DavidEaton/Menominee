using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.CreditCards;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.CreditCards
{
    public class CreditCardsController : ApplicationController
    {
        private readonly ICreditCardRepository repository;
        //private readonly string BasePath = "/api/creditcards";

        public CreditCardsController(ICreditCardRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // api/creditcards/listing
        [Route("listing")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CreditCardToReadInList>>> GetCreditCardListAsync()
        {
            var results = await repository.GetCreditCardListAsync();
            return Ok(results);
        }

        // api/creditcards/1
        [HttpGet("{id:long}", Name = "GetCreditCardAsync")]
        public async Task<ActionResult<CreditCardToRead>> GetCreditCardAsync(long id)
        {
            var result = await repository.GetCreditCardAsync(id);

            if (result == null)
                return NotFound();

            return result;
        }

        // api/creditcards/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCreditCardAsync(long id, CreditCardToWrite creditCard)
        {
            if (!await repository.CreditCardExistsAsync(id))
                return NotFound($"Could not find Credit Card to update: {creditCard.Name}");

            //1) Get domain entity from repository
            var creditCardFromRepository = await repository.GetCreditCardEntityAsync(id);

            // 2) Update domain entity with data in data transfer object(DTO)
            creditCardFromRepository.Name = creditCard.Name;
            creditCardFromRepository.FeeType = creditCard.FeeType;
            creditCardFromRepository.Fee = creditCard.Fee;
            creditCardFromRepository.IsAddedToDeposit = creditCard.IsAddedToDeposit;
            //cc.Processor = ccDto.Processor;

            // Update the objects ObjectState and sych the EF Change Tracker
            // 3) Set entity's TrackingState to Modified
            creditCardFromRepository.SetTrackingState(TrackingState.Modified);

            // 4) FixTrackingState: moves entity state tracking into the context
            repository.FixTrackingState();

            await repository.UpdateCreditCardAsync(creditCardFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CreditCardToRead>> AddCreditCardAsync(CreditCardToWrite ccDto)
        {
            // 1. Convert dto to domain entity
            var cc = new CreditCard()
            {
                Name = ccDto.Name,
                FeeType = ccDto.FeeType,
                Fee = ccDto.Fee,
                IsAddedToDeposit = ccDto.IsAddedToDeposit
                //Processor = ccDto.Processor
            };

            // 2. Add domain entity to repository
            await repository.AddCreditCardAsync(cc);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4.Return new Id from database to consumer after save
            return CreatedAtRoute("GetCreditCardAsync",
                                  new { id = cc.Id },
                                  CreditCardHelper.CreateCreditCard(cc));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCreditCardAsync(long id)
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
