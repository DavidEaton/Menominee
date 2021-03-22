using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IOrganizationRepository
    {
        void Create(OrganizationCreateDto entity);
        Task<IEnumerable<OrganizationReadDto>> GetOrganizationsAsync();
        Task<OrganizationReadDto> GetOrganizationAsync(int id);
        Task<Organization> GetOrganizationEntityAsync(int id);
        Task<IEnumerable<OrganizationsInListDto>> GetOrganizationsListAsync();
        void UpdateOrganizationAsync(Organization entity);
        void Delete(Organization entity);
        void FixTrackingState();
        Task<bool> OrganizationExistsAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
