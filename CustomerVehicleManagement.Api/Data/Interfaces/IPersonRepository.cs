using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task CreatePersonAsync(PersonCreateDto entity);
        void Delete(Person entity);
        void FixTrackingState();
        Task<bool> PersonExistsAsync(int id);
        void UpdatePersonAsync(PersonUpdateDto entity);
        Task<IEnumerable<PersonReadDto>> GetPersonsAsync();
        Task<IEnumerable<PersonInListDto>> GetPersonsListAsync();
        Task<PersonReadDto> GetPersonAsync(int id);
        Task<Person> GetPersonEntityAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
