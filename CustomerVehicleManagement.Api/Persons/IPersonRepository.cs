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
        void UpdatePersonAsync(Person personUpdateDto);
        Task<IReadOnlyList<PersonReadDto>> GetPersonsAsync();
        Task<IReadOnlyList<PersonInListDto>> GetPersonsListAsync();
        Task<PersonReadDto> GetPersonAsync(long id);
        Task<Person> GetPersonEntityAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
