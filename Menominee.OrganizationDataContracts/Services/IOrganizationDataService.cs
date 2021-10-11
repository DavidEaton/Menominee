using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.OrganizationDataContracts.Services
{
    public interface IOrganizationDataService
    {
        Task<IReadOnlyList<OrganizationToReadInList>> GetAllOrganizations();
        Task<OrganizationToRead> GetOrganization(long id);
        Task<OrganizationToRead> AddOrganization(OrganizationToAdd organization);
        Task UpdateOrganization(OrganizationToEdit organization, long id);
    }
}
