using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IPersonDataService
    {
        Task<IReadOnlyList<PersonInListDto>> GetAllPersons();
        Task<PersonReadDto> GetPersonDetails(long id);
        Task<PersonReadDto> AddPerson(PersonAddDto person);
    }
}
