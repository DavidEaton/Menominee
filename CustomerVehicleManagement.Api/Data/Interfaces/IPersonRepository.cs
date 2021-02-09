using CustomerVehicleManagement.Api.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        void AddPerson(PersonCreateDto entity);
        void DeletePerson(PersonReadDto entity);
        void FixState();
        Task<bool> PersonExistsAsync(int id);
        Task<PersonReadDto> UpdatePersonAsync(PersonUpdateDto entity);
        Task<IEnumerable<PersonReadDto>> GetPersonsAsync();
        Task<IEnumerable<PersonListDto>> GetPersonsListAsync();
        Task<PersonReadDto> GetPersonAsync(int id);
        Task<bool> SaveChangesAsync(PersonCreateDto person);
        Task<bool> SaveChangesAsync();
    }
}
