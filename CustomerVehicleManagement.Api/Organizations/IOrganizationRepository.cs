using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    public interface IOrganizationRepository
    {
        Task AddOrganizationAsync(Organization entity);
        Task<IReadOnlyList<OrganizationReadDto>> GetOrganizationsAsync();
        Task<OrganizationReadDto> GetOrganizationAsync(long id);
        Task<IReadOnlyList<OrganizationInListDto>> GetOrganizationsListAsync();
        void UpdateOrganizationAsync(Organization entity);
        Task DeleteOrganizationAsync(long id);
        void FixTrackingState();
        Task<bool> OrganizationExistsAsync(long id);
        Task<bool> SaveChangesAsync();
        Task<Organization> GetOrganizationEntityAsync(long id);
    }
}
