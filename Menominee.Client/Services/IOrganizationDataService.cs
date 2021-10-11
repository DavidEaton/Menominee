using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IOrganizationDataService
    {
        Task<IReadOnlyList<OrganizationToReadInList>> GetAllOrganizations();
        Task<OrganizationToRead> GetOrganizationDetails(long id);
        Task<OrganizationToRead> AddOrganization(OrganizationToAdd Organization);
    }
}
