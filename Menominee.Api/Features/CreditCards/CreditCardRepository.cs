using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.CreditCards
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(CreditCard creditCard)
        {
            if (creditCard is not null)
                context.Attach(creditCard);
        }

        public async Task<bool> CreditCardExistsAsync(long id)
        {
            return await context.CreditCards
                .AnyAsync(creditCard => creditCard.Id == id);
        }

        public void Delete(CreditCard creditCard)
        {
            context.Remove(creditCard);
        }

        public async Task<CreditCardToRead> GetAsync(long id)
        {
            var creditCardFromContext = await context.CreditCards
                .AsNoTracking()
                .FirstOrDefaultAsync(creditCard =>
                creditCard.Id == id);

            return creditCardFromContext is not null
                ? CreditCardHelper.CreateCreditCard(creditCardFromContext)
                : null;
        }

        public async Task<CreditCard> GetEntityAsync(long id)
        {
            return await context.CreditCards
                .FirstOrDefaultAsync(creditCard => creditCard.Id == id);
        }

        public async Task<IReadOnlyList<CreditCardToReadInList>> GetListAsync()
        {
            IReadOnlyList<CreditCard> creditCards = await context.CreditCards.ToListAsync();

            return creditCards
                .Select(creditCard => CreditCardHelper.CreateCreditCardInList(creditCard))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}