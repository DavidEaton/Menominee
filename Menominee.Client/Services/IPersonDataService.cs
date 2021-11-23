using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IPersonDataService
    {
        Task<IReadOnlyList<PersonToReadInList>> GetAllPersons();
        Task<PersonToRead> GetPersonDetails(long id);
        Task<PersonToRead> AddPerson(PersonToWrite person);
    }
}
