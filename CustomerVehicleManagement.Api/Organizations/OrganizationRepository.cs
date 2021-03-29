using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using CustomerVehicleManagement.Api.Utilities;
using AutoMapper;
using SharedKernel.ValueObjects;
using CustomerVehicleManagement.Api.Data;

namespace CustomerVehicleManagement.Api.Organizations
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


        public async Task AddOrganizationAsync(OrganizationCreateDto organizationCreateDto)
        {
            Organization organization = null;

            if (organizationCreateDto != null)
            {
                var organizationNameOrError = OrganizationName.Create(organizationCreateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return;

                organization = new Organization(organizationNameOrError.Value);

                if (organizationCreateDto.Address != null)
                    organization.SetAddress(organizationCreateDto.Address);

                if (organizationCreateDto.Contact != null)
                    organization.SetContact(new Person(organizationCreateDto.Contact.Name, organizationCreateDto.Contact.Gender));

                if (organizationCreateDto.Phones != null)
                    organization.SetPhones(organizationCreateDto.Phones);

                if (organizationCreateDto.Emails != null)
                    organization.SetEmails(organizationCreateDto.Emails);
            }

            if (organization != null)
                await context.AddAsync(organization);
        }

        public void DeleteOrganization(Organization organization)
        {
            context.Remove(organization);
        }

        public async Task<OrganizationReadDto> GetOrganizationAsync(int id)
        {
            var organizationFromContext =
                await context.Organizations
                             .Include(organization => organization.Phones)
                             .Include(organization => organization.Emails)
                             .Include(organization => organization.Contact)
                                 .ThenInclude(contact => contact.Phones)
                             .Include(organization => organization.Contact)
                                 .ThenInclude(contact => contact.Emails)
                             .FirstOrDefaultAsync(organization => organization.Id == id);

            return mapper.Map<OrganizationReadDto>(organizationFromContext);
        }

        public async Task<IEnumerable<OrganizationReadDto>> GetOrganizationsAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext = 
                await context.Organizations
                             .Include(organization => organization.Phones)
                             .Include(organization => organization.Emails)
                             .ToListAsync();

            return mapper.Map<IEnumerable<OrganizationReadDto>>(organizationsFromContext);
        }


        public async Task<Organization> GetOrganizationEntityAsync(int id)
        {
            var organizationFromContext = context.Organizations.FindAsync(id);

            return await (organizationFromContext);
        }

        public async Task<IEnumerable<OrganizationsInListDto>> GetOrganizationsListAsync()
        {
            IReadOnlyList<Organization> organizations = await context.Organizations
                                                               .Include(organization => organization.Contact.Phones)
                                                               .ToListAsync();

            List<OrganizationsInListDto> dtos = organizations.Select(organization => new OrganizationsInListDto
            {
                Id = organization.Id,
                Name = organization.Name.Value,
                ContactName = organization?.Contact?.Name.LastFirstMiddle,
                ContactPrimaryPhone = ContactableHelpers.GetPrimaryPhone(organization?.Contact),

                AddressLine = organization?.Address?.AddressLine,
                City = organization?.Address?.City,
                State = organization?.Address?.City,
                PostalCode = organization?.Address?.PostalCode,

                Notes = organization.Notes,
                PrimaryPhone = ContactableHelpers.GetPrimaryPhone(organization),
                PrimaryPhoneType = ContactableHelpers.GetPrimaryPhoneType(organization)
            }).ToList();

            return dtos;
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

