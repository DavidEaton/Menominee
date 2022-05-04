using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.CreditCards;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.CreditCards
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ApplicationDbContext context;

        public CreditCardRepository(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddCreditCardAsync(CreditCard creditCard)
        {
            Guard.ForNull(creditCard, "Credit Card");

            if (await CreditCardExistsAsync(creditCard.Id))
                throw new Exception("Credit Card already exists");

            await context.AddAsync(creditCard);
        }

        public async Task<bool> CreditCardExistsAsync(long id)
        {
            return await context.CreditCards.AnyAsync(cc => cc.Id == id);
        }

        public async Task DeleteCreditCardAsync(long id)
        {
            var cc = await context.CreditCards
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(cc => cc.Id == id);

            Guard.ForNull(cc, "Credit Card");

            context.Remove(cc);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<CreditCardToRead> GetCreditCardAsync(long id)
        {
            var ccFromContext = await context.CreditCards
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(cc => cc.Id == id);

            Guard.ForNull(ccFromContext, "Credit Card");

            return CreditCardHelper.Transform(ccFromContext);
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
                .Select(cc => CreditCardHelper.TransformToListItem(cc))
                .ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<CreditCard> UpdateCreditCardAsync(CreditCard creditCard)
        {
            Guard.ForNull(creditCard, "Credit Card");

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

            return null;
        }
    }
}
