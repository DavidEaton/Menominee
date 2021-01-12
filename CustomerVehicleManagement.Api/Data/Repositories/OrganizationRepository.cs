using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext context;
        public OrganizationRepository()
        {
        }

        public OrganizationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AddOrganization(Organization person)
        {
            context.Add(person);
        }

        public void DeleteOrganization(Organization person)
        {
            context.Remove(person);
        }

        public async Task<Organization> GetOrganizationAsync(int id)
        {
            return await context.Organizations
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Organization[]> GetOrganizationsAsync()
        {
            return await context.Organizations
                // Tracking is not needed (and expensive) for this disconnected data collection
                .AsNoTracking()
                .Include(o => o.Address)
                .ToArrayAsync();
        }

        public async Task<bool> OrganizationExistsAsync(int id)
        {
            return await context.Organizations
                .AnyAsync(o => o.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Organization organization)
        {
            // Mark person EF tracking state = modified via dbContext:
            context.Organizations
                .Update(organization);

            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void FixState()
        {
            context.FixState();
        }

        public async Task<Organization> UpdateOrganizationAsync(Organization organization)
        {
            if (organization == null)
                throw new NullReferenceException("Organization Id missing.");

            context.Entry(organization).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrganizationExistsAsync(organization.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }

    }
}

