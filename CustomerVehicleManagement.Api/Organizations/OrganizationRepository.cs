using AutoMapper;
using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task AddOrganizationAsync(Organization organization)
        {
            if (organization != null)
                await context.AddAsync(organization);
        }

        public async Task DeleteOrganizationAsync(int id)
        {
            var organizationFromContext = await context.Organizations.FindAsync(id);
            context.Remove(organizationFromContext);
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

        public async Task<IReadOnlyList<OrganizationReadDto>> GetOrganizationsAsync()
        {
            IList<OrganizationReadDto> list = new List<OrganizationReadDto>();

            IReadOnlyList<Organization> organizationsFromContext =
                await context.Organizations
                             .Include(organization => organization.Phones)
                             .Include(organization => organization.Emails)
                             .Include(organization => organization.Contact)
                                 .ThenInclude(contact => contact.Phones)
                                 .Include(contact => contact.Emails)
                             .ToListAsync();

            foreach (var organization in organizationsFromContext)
            {
                list.Add(new OrganizationReadDto()
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    Address = organization.Address,
                    Notes = organization.Notes,

                    Phones = organization.Phones.Select(x => new PhoneReadDto()
                    {
                        Number = x.Number,
                        PhoneType = x.PhoneType.ToString(),
                        IsPrimary = x.IsPrimary
                    }).ToArray(),

                    Emails = organization.Emails.Select(x => new EmailReadDto()
                    {
                        Address = x.Address,
                        IsPrimary = x.IsPrimary
                    }).ToArray()
                });
            }
            return (IReadOnlyList<OrganizationReadDto>)list;
        }

            //var organizationReadDto = organizationsFromContext.Select(organization => new OrganizationReadDto()
            //{
            //    Id = organization.Id,
            //    Name = organization.Name.Name,
            //    Address = organization.Address,
            //    Notes = organization.Notes,

            //    Phones = organization.Phones.Select(x => new PhoneReadDto()
            //    {
            //        Number = x.Number,
            //        PhoneType = x.PhoneType.ToString(),
            //        IsPrimary = x.IsPrimary
            //    }).ToArray(),

            //    Emails = organization.Emails.Select(x => new EmailReadDto()
            //    {
            //        Address = x.Address,
            //        IsPrimary = x.IsPrimary
            //    }).ToArray()

            //}).ToArray();


    public async Task<Organization> GetOrganizationEntityAsync(int id)
    {
        var organizationFromContext = context.Organizations
                                             .Include(organization => organization.Phones)
                                             .Include(organization => organization.Emails)
                                             .Include(organization => organization.Contact)
                                                 .ThenInclude(contact => contact.Phones)
                                             .Include(organization => organization.Contact)
                                                 .ThenInclude(contact => contact.Emails)
                                             .FirstOrDefaultAsync(organization => organization.Id == id);

        return await (organizationFromContext);
    }

    public async Task<IReadOnlyList<OrganizationsInListDto>> GetOrganizationsListAsync()
    {
        IReadOnlyList<Organization> organizations = await context.Organizations
                                                                 .Include(organization => organization.Contact)
                                                                    .ThenInclude(contact => contact.Phones)
                                                                 .Include(organization => organization.Contact)
                                                                    .ThenInclude(contact => contact.Emails)
                                                                 .ToListAsync();

        List<OrganizationsInListDto> dtos = organizations.Select(organization => new OrganizationsInListDto
        {
            Id = organization.Id,
            Name = organization.Name.Name,
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

