using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.CreditCards
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddCreditCardAsync(CreditCard creditCard)
        {
            if (creditCard is not null)
            {
                if (await CreditCardExistsAsync(creditCard.Id))
                    throw new Exception("Credit Card already exists");

                await context.AddAsync(creditCard);
            }
        }

        public async Task<bool> CreditCardExistsAsync(long id)
        {
            return await context.CreditCards.AnyAsync(cc => cc.Id == id);
        }

        public async Task DeleteCreditCardAsync(long id)
        {
            var creditCard = await context.CreditCards
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(creditCard =>
                                    creditCard.Id == id);

            if (creditCard is not null)
                context.Remove(creditCard);
        }

        public async Task<CreditCardToRead> GetCreditCardAsync(long id)
        {
            var creditCardFromContext = await context.CreditCards
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(creditCard =>
                                                creditCard.Id == id);

            return creditCardFromContext is not null
                ? CreditCardHelper.CreateCreditCard(creditCardFromContext)
                : null;
        }

        public async Task<CreditCard> GetCreditCardEntityAsync(long id)
        {
            return await context.CreditCards.FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public async Task<IReadOnlyList<CreditCardToReadInList>> GetCreditCardListAsync()
        {
            IReadOnlyList<CreditCard> creditCards = await context.CreditCards.ToListAsync();

            return creditCards
                //.Select(cc => CreditCardToReadInList.ConvertToDto(cc))
                .Select(cc => CreditCardHelper.CreateCreditCardInList(cc))
                .ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<CreditCard> UpdateCreditCardAsync(CreditCard creditCard)
        {
            if (creditCard is not null)
            {
                // Tracking IS needed for commands for disconnected data collections
                context.Entry(creditCard).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CreditCardExistsAsync(creditCard.Id))
                        return null; // something that tells the controller to return NotFound();
                    throw;
                }
            }

            return null;
        }
    }
}
