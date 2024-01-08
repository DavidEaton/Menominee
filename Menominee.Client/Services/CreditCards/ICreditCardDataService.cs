using CSharpFunctionalExtensions;
using Menominee.Shared.Models.CreditCards;
using Menominee.Shared.Models.Http;

namespace Menominee.Client.Services.CreditCards
{
    public interface ICreditCardDataService
    {
        Task<Result<IReadOnlyList<CreditCardToReadInList>>> GetAllAsync();
        Task<Result<CreditCardToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(CreditCardToWrite creditCard);
        Task<Result> UpdateAsync(CreditCardToWrite creditCard);
    }
}
