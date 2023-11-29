using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Contactables.Persons
{
    public interface IPersonRepository
    {
        void Add(Person entity);
        void Delete(Person entity);
        Task<IReadOnlyList<PersonToRead>> GetAllAsync();
        Task<IReadOnlyList<PersonToReadInList>> GetListAsync();
        Task<PersonToRead> GetAsync(long id);
        Task<Person> GetEntityAsync(long id);
        Task SaveChangesAsync();
        void DeletePhone(Phone entity);
        void DeleteEmail(Email entity);
    }
}