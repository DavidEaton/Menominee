using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.UiExperiments.Services
{
    public interface IPersonDataService
    {
        Task<IReadOnlyList<PersonReadDto>> GetAllPersons();
        Task<PersonReadDto> GetPersonDetails(long id);
        Task<PersonReadDto> AddPerson(PersonAddDto person);
        Task UpdatePerson(PersonUpdateDto person);
        Task DeletePerson(long id);
    }
}