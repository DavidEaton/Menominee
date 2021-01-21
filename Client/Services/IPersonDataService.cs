using Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IPersonDataService
    {
        Task<IEnumerable<PersonFlatDto>> GetAllPersons();
        Task<PersonFlatDto> GetPersonDetails(int id);
        Task<PersonFlatDto> AddPerson(PersonAddDto person);
        Task UpdatePerson(PersonFlatDto person);
        Task DeletePerson(int id);
    }
}
