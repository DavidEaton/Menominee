using Menominee.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IPersonDataService
    {
        Task<IEnumerable<PersonFlatDto>> GetAllPersons();
        Task<PersonFlatDto> GetPersonDetails(int id);
        Task<PersonFlatDto> AddPerson(PersonCreateDto person);
        Task<int> GetPersonsTotal();
        Task UpdatePerson(PersonFlatDto person);
        Task DeletePerson(int id);
    }
}
