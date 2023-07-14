﻿using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.CreditCards
{
    public interface ICreditCardRepository
    {
        Task AddCreditCardAsync(CreditCard creditCard);
        Task<CreditCard> GetCreditCardEntityAsync(long id);
        Task<CreditCardToRead> GetCreditCardAsync(long id);
        Task<IReadOnlyList<CreditCardToReadInList>> GetCreditCardListAsync();
        Task<CreditCard> UpdateCreditCardAsync(CreditCard CreditCard);
        Task DeleteCreditCardAsync(long id);
        Task<bool> CreditCardExistsAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}