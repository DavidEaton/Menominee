using Migrations.Core.Entities;
using System.Threading.Tasks;

namespace Migrations.Api.Data.Interfaces
{
    public interface IOrganizationRepository
    {
        void AddOrganization(Organization entity);
        void DeleteOrganization(Organization entity);
        void FixState();
        Task<bool> OrganizationExistsAsync(int id);
        Task<Organization> UpdateOrganizationAsync(Organization entity);
        Task<Organization[]> GetOrganizationsAsync();
        Task<Organization> GetOrganizationAsync(int id);
        Task<bool> SaveChangesAsync(Organization person);
        Task<bool> SaveChangesAsync();
    }
}
