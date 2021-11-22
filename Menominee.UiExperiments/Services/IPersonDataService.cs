using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.UiExperiments.Services
{
    public interface IPersonDataService
    {
        Task<IReadOnlyList<PersonToRead>> GetAllPersons();
        Task<PersonToRead> GetPersonDetails(long id);
        Task<PersonToRead> AddPerson(PersonToWrite person);
        Task UpdatePerson(PersonToWrite person);
        Task DeletePerson(long id);
    }
}