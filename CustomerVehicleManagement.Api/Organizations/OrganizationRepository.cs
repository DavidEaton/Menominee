using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext context;

        public OrganizationRepository(
            ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddOrganizationAsync(Organization organization)
        {
            if (organization != null)
                await context.AddAsync(organization);
        }

        public async Task DeleteOrganizationAsync(long id)
        {
            var organizationFromContext = await context.Organizations.FindAsync(id);
            context.Remove(organizationFromContext);
        }

        public async Task<OrganizationToRead> GetOrganizationAsync(long id)
        {
            var organizationFromContext =
                await context.Organizations
                             .Include(organization => organization.Phones
                                .OrderByDescending(phone => phone.IsPrimary))
                             .AsNoTracking()

                             .Include(organization => organization.Emails
                                .OrderByDescending(email => email.IsPrimary))
                             .AsNoTracking()

                             .Include(organization => organization.Contact.Phones)
                             .AsNoTracking()

                             .Include(organization => organization.Contact.Emails)
                             .AsNoTracking()
                             
                             .FirstOrDefaultAsync(organization => organization.Id == id);

            return OrganizationToRead.ConvertToDto(organizationFromContext);
        }

        public async Task<IReadOnlyList<OrganizationToRead>> GetOrganizationsAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext =
                await context.Organizations
                             .Include(organization => organization.Phones
                                .OrderByDescending(phone => phone.IsPrimary))
                             .AsNoTracking()

                             .Include(organization => organization.Emails
                                .OrderByDescending(email => email.IsPrimary))
                             .AsNoTracking()

                             .Include(organization => organization.Contact.Phones)
                             .AsNoTracking()

                             .Include(organization => organization.Contact.Emails)
                             .AsNoTracking()

                             .ToListAsync();

            return organizationsFromContext
                .Select(organization =>
                        OrganizationToRead.ConvertToDto(organization))
                .ToList();

        }

        public async Task<Organization> GetOrganizationEntityAsync(long id)
        {
            // Prefer FindAsync() over Single() or First() for single objects (non-collections);
            // FindAsync() checks the Identity Map Cache before making a trip to the database.
            var organizationFromContext = context.Organizations
                                             .Include(organization => organization.Phones)
                                             .Include(organization => organization.Emails)
                                             .Include(organization => organization.Contact)
                                                .ThenInclude(contact => contact.Phones)
                                             .Include(contact => contact.Emails)
                                             .FirstOrDefaultAsync(organization => organization.Id == id);

            return await organizationFromContext;
        }

        public async Task<IReadOnlyList<OrganizationToReadInList>> GetOrganizationsListAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext =
                await context.Organizations
                             .Include(organization => organization.Phones)
                                .AsNoTracking()

                             .Include(organization => organization.Emails)
                                .AsNoTracking()

                             // Contact
                             .Include(organization => organization.Contact.Phones
                                .Where(phone => phone.IsPrimary == true))
                                .AsNoTracking()
                             .Include(contact => contact.Emails
                                .Where(email => email.IsPrimary == true))
                                .AsNoTracking()

                             .ToArrayAsync();

            return organizationsFromContext.
                Select(organization =>
                       OrganizationToReadInList.ConvertToDto(organization))
               .OrderBy(organization => organization.Name)
               .ToList();
        }

        public void UpdateOrganizationAsync(Organization organization)
        {
            // No code in this implementation.
        }

        public async Task<bool> OrganizationExistsAsync(long id)
        {
            return await context.Organizations
                .AnyAsync(o => o.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void FixTrackingState()
        {
            context.FixState();
        }
    }
}

