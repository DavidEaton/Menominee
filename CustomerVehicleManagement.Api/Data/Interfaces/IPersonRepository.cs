using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        void AddPerson(PersonCreateDto entity);
        void DeletePerson(Person entity);
        void FixState();
        Task<bool> PersonExistsAsync(int id);
        void UpdatePersonAsync(Person entity);
        Task<IEnumerable<PersonReadDto>> GetPersonsAsync();
        Task<IEnumerable<PersonListDto>> GetPersonsListAsync();
        Task<Person> GetPersonAsync(int id);
        Task<PersonReadDto> SaveChangesAsync(PersonCreateDto person);
        Task<bool> SaveChangesAsync();
    }
}
