using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Organizations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddOrganizationAsync(Organization organization)
        {
            if (organization is not null)
                await context.AddAsync(organization);
        }

        public void DeleteOrganization(Organization organization)
        {
            context.Remove(organization);
        }

        public async Task<OrganizationToRead> GetOrganizationAsync(long id)
        {
            var organizationFromContext = await context.Organizations
                .Include(organization => organization.Phones)
                .Include(organization => organization.Emails)

                .Include(organization => organization.Contact)
                    .ThenInclude(contact => contact.Emails)
                .Include(organization => organization.Contact)
                    .ThenInclude(contact => contact.Phones)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(organization => organization.Id == id);

            return OrganizationHelper.ConvertToReadDto(organizationFromContext);
        }

        public async Task<IReadOnlyList<OrganizationToRead>> GetOrganizationsAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext = await context.Organizations
                .Include(organization => organization.Phones
                    .OrderByDescending(phone => phone.IsPrimary))

                .Include(organization => organization.Emails
                    .OrderByDescending(email => email.IsPrimary))

                .Include(organization => organization.Contact.Phones)
                .Include(organization => organization.Contact.Emails)

                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            return organizationsFromContext
                .Select(organization =>
                        OrganizationHelper.ConvertToReadDto(organization))
                .ToList();
        }

        public async Task<IReadOnlyList<OrganizationToReadInList>> GetOrganizationsListAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext = await context.Organizations
                .Include(organization => organization.Phones
                    .Where(phone => phone.IsPrimary == true))
                .Include(organization => organization.Emails
                    .Where(email => email.IsPrimary == true))

                .Include(organization => organization.Contact.Phones
                    .Where(phone => phone.IsPrimary == true))
                .Include(organization => organization.Contact.Emails
                    .Where(email => email.IsPrimary == true))

                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return organizationsFromContext
                .Select(organization =>
                       OrganizationHelper.ConvertToReadInListDto(organization))
                .OrderBy(organization => organization.Name)
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
                    .ThenInclude(contact => contact.Emails)
                .Include(organization => organization.Contact)
                    .ThenInclude(contact => contact.Phones)
                .AsSplitQuery()
                .FirstOrDefaultAsync(organization => organization.Id == id);

            return await organizationFromContext;
        }

        public async Task<bool> OrganizationExistsAsync(long id)
        {
            return await context.Organizations
                .AnyAsync(organization => organization.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}