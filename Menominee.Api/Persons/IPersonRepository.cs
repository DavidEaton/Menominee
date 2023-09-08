using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Persons
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person entity);
        void DeletePerson(Person entity);
        Task<bool> PersonExistsAsync(long id);
        Task<IReadOnlyList<PersonToRead>> GetPersonsAsync();
        Task<IReadOnlyList<PersonToReadInList>> GetPersonsListAsync();
        Task<PersonToRead> GetPersonAsync(long id);
        Task<Person> GetPersonEntityAsync(long id);
        Task SaveChangesAsync();
        void DeletePhone(Phone phone);
        void DeleteEmail(Email email);
    }
}