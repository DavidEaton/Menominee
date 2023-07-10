using Menominee.Domain.Entities;
using Menominee.Shared.Models.Organizations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Organizations
{
    public interface IOrganizationRepository
    {
        Task AddOrganizationAsync(Organization entity);
        Task<IReadOnlyList<OrganizationToRead>> GetOrganizationsAsync();
        Task<OrganizationToRead> GetOrganizationAsync(long id);
        Task<IReadOnlyList<OrganizationToReadInList>> GetOrganizationsListAsync();
        void DeleteOrganization(Organization entity);
        Task<bool> OrganizationExistsAsync(long id);
        Task SaveChangesAsync();
        Task<Organization> GetOrganizationEntityAsync(long id);
    }
}