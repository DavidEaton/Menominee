using Menominee.Shared.Models.CreditCards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.CreditCards
{
    public interface ICreditCardDataService
    {
        Task<IReadOnlyList<CreditCardToReadInList>> GetAllCreditCardsAsync();
        Task<CreditCardToRead> GetCreditCardAsync(long id);
        Task<CreditCardToRead> AddCreditCardAsync(CreditCardToWrite creditCard);
        Task UpdateCreditCardAsync(CreditCardToWrite creditCard, long id);
    }
}
