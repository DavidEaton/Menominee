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
        Task<bool> PersonExistsAsync(int id);
        void UpdatePersonAsync(Person personUpdateDto);
        Task<IReadOnlyList<PersonReadDto>> GetPersonsAsync();
        Task<IReadOnlyList<PersonInListDto>> GetPersonsListAsync();
        Task<PersonReadDto> GetPersonAsync(int id);
        Task<Person> GetPersonEntityAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
