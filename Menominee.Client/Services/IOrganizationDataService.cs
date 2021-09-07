using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IOrganizationDataService
    {
        Task<IReadOnlyList<OrganizationInListDto>> GetAllOrganizations();
        Task<OrganizationReadDto> GetOrganizationDetails(int id);
        Task<OrganizationReadDto> AddOrganization(OrganizationAddDto Organization);
    }
}
