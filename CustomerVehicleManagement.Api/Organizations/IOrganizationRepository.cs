using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    public interface IOrganizationRepository
    {
        Task AddOrganizationAsync(Organization entity);
        Task<IReadOnlyList<OrganizationToRead>> GetOrganizationsAsync();
        Task<OrganizationToRead> GetOrganizationAsync(long id);
        Task<IReadOnlyList<OrganizationToReadInList>> GetOrganizationsListAsync();
        void UpdateOrganizationAsync(Organization entity);
        void DeleteOrganization(Organization entity);
        void FixTrackingState();
        Task<bool> OrganizationExistsAsync(long id);
        Task SaveChangesAsync();
        Task<Organization> GetOrganizationEntityAsync(long id);
    }
}
