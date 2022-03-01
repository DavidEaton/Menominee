using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person entity);
        void DeletePerson(Person entity);
        void FixTrackingState();
        Task<bool> PersonExistsAsync(long id);
        void UpdatePersonAsync(Person person);
        Task<IReadOnlyList<PersonToRead>> GetPersonsAsync();
        Task<IReadOnlyList<PersonToReadInList>> GetPersonsListAsync();
        Task<PersonToRead> GetPersonAsync(long id);
        Task<Person> GetPersonEntityAsync(long id);
        Task SaveChangesAsync();
    }
}
