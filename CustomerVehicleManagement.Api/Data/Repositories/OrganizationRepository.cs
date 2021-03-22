using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using CustomerVehicleManagement.Api.Data.Dtos;
using System.Linq;
using CustomerVehicleManagement.Api.Utilities;
using AutoMapper;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public OrganizationRepository(
            AppDbContext context,
            IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        public void Create(OrganizationCreateDto organizationCreateDto)
        {
            Organization organization = null;

            if (organizationCreateDto != null)
            {
                organization = new Organization(organizationCreateDto.Name);
                organization.SetAddress(organizationCreateDto.Address);
                organization.SetContact(new Person(organizationCreateDto.Contact.Name, organizationCreateDto.Contact.Gender));
                organization.SetPhones(organizationCreateDto.Phones);
                organization.SetEmails(organizationCreateDto.Emails);
            }

            if (organization != null)
                context.Add(organization);
        }

        public void Delete(Organization organization)
        {
            context.Remove(organization);
        }

        public async Task<OrganizationReadDto> GetOrganizationAsync(int id)
        {
            var organizationFromContext = await context.Organizations
            .Include(organization => organization.Phones)
            .Include(organization => organization.Emails)
            .Include(organization => organization.Contact)
                .ThenInclude(contact => contact.Phones)
                .Include(contact => contact.Emails)
            .FirstOrDefaultAsync(organization => organization.Id == id);

            return mapper.Map<OrganizationReadDto>(organizationFromContext);
        }

        public async Task<IEnumerable<OrganizationReadDto>> GetOrganizationsAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext = await context.Organizations.ToListAsync();

            return organizationsFromContext
                .Select(organization => DtoHelpers.ConvertOrganizationDomainToReadDto(organization))
                .ToList();
        }


        public async Task<Organization> GetOrganizationEntityAsync(int id)
        {
            var organizationFromContext = context.Organizations.FindAsync(id);

            return await (organizationFromContext);
        }

        public async Task<IEnumerable<OrganizationsInListDto>> GetOrganizationsListAsync()
        {
            var organizationsFromContext = context.Organizations
                                                  .Include(x => x.Contact.Phones)
                                                  .ToArray();

            var organizationsList = new List<OrganizationsInListDto>();

            foreach (var organization in organizationsFromContext)
            {
                organizationsList.Add(DtoHelpers.CreateOrganizationsListDtoFromDomain(organization));
            }

            return await Task.FromResult(organizationsList);
        }

        public void UpdateOrganizationAsync(Organization organization)
        {
            // No code in this implementation.
        }

        public async Task<bool> OrganizationExistsAsync(int id)
        {
            return await context.Organizations
                .AnyAsync(o => o.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void FixTrackingState()
        {
            context.FixState();
        }
    }
}

