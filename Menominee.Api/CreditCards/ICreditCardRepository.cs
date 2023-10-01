using Menominee.Domain.Entities;
using Menominee.Shared.Models.CreditCards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.CreditCards
{
    public interface ICreditCardRepository
    {
        void Add(CreditCard entity);
        void Delete(CreditCard entity);
        Task<CreditCard> GetEntityAsync(long id);
        Task<CreditCardToRead> GetAsync(long id);
        Task<IReadOnlyList<CreditCardToReadInList>> GetListAsync();
        Task SaveChangesAsync();
    }
}
