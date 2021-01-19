using Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IPersonDataService
    {
        Task<IEnumerable<PersonDto>> GetAllPersons();
        Task<PersonDto> GetPerson(int id);
        Task<PersonDto> AddPerson(PersonDto person);
        Task UpdatePerson(PersonDto person);
        Task DeletePerson(int id);
    }
}
