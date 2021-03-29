using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    public interface IOrganizationRepository
    {
        Task AddOrganizationAsync(OrganizationCreateDto entity);
        Task<IEnumerable<OrganizationReadDto>> GetOrganizationsAsync();
        Task<OrganizationReadDto> GetOrganizationAsync(int id);
        Task<Organization> GetOrganizationEntityAsync(int id);
        Task<IEnumerable<OrganizationsInListDto>> GetOrganizationsListAsync();
        void UpdateOrganizationAsync(Organization entity);
        void DeleteOrganization(Organization entity);
        void FixTrackingState();
        Task<bool> OrganizationExistsAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
