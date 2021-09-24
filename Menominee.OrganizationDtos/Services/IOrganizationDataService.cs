using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Services
{
    public interface IOrganizationDataService
    {
        Task<IReadOnlyList<OrganizationInListDto>> GetAllOrganizations();
        Task<OrganizationReadDto> GetOrganization(long id);
        Task<OrganizationReadDto> AddOrganization(OrganizationAddDto organization);
        Task UpdateOrganization(OrganizationUpdateDto organization, long id);
    }
}
