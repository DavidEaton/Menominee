using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.CreditCards;

namespace Menominee.Client.Services.CreditCards
{
    public interface ICreditCardDataService
    {
        Task<Result<IReadOnlyList<CreditCardToReadInList>>> GetAllCreditCardsAsync();
        Task<Result<CreditCardToRead>> GetCreditCardAsync(long id);
        Task<Result<PostResponse>> AddCreditCardAsync(CreditCardToWrite creditCard);
        Task<Result> UpdateCreditCardAsync(CreditCardToWrite creditCard);
    }
}
