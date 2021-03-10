using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using CustomerVehicleManagement.Api.Data.Models;
using System.Linq;
using CustomerVehicleManagement.Api.Utilities;

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
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
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
            var organization = context.Organizations.Find(id);

            if (organization == null)
                return null;

            var dto = new OrganizationReadDto
            {
                Id = organization.Id,
                Name = organization.Name,
                AddressLine = organization?.Address?.AddressLine,
                City = organization?.Address?.City,
                State = organization?.Address?.State,
                PostalCode = organization?.Address?.PostalCode,
                Contact = (organization.Contact == null) ? null : new ContactReadDto
                {
                    Id = organization.Contact.Id,
                    Name = organization.Contact.Name.LastFirstMiddle,
                    Phones = organization.Contact?.Phones.Select(phone => new PhoneReadDto
                    {
                        Number = phone.Number,
                        PhoneType = phone.PhoneType.ToString(),
                        Primary = phone.IsPrimary
                    })
                },
                Notes = organization?.Notes,
                Phones = ContactableHelpers.MapDomainPhoneToReadDto(organization.Phones)
            };

            return await Task.FromResult(dto);
        }

        public async Task<IEnumerable<OrganizationReadDto>> GetOrganizationsAsync()
        {
            IReadOnlyList<Organization> organizationsFromContext = await context.Organizations.ToListAsync();

            List<OrganizationReadDto> dtos = organizationsFromContext.Select(o => new OrganizationReadDto
            {
                Id = o.Id,
                Name = o.Name,
                Contact = (o.Contact == null) ? null : new ContactReadDto
                {
                    Id = o.Contact.Id,
                    Name = o.Contact.Name.LastFirstMiddle,
                    Phones = o.Contact?.Phones.Select(x => new PhoneReadDto
                    {
                        Number = x.Number,
                        PhoneType = x.PhoneType.ToString(),
                        Primary = x.IsPrimary
                    })
                },
                AddressLine = o?.Address?.AddressLine,
                City = o?.Address?.City,
                State = o?.Address?.State,
                PostalCode = o?.Address?.PostalCode,
                Notes = o?.Notes
            }).ToList();

            return dtos;
        }

        public async Task<Organization> GetOrganizationEntityAsync(int id)
        {
            var organizationFromContext = context.Organizations.Find(id);

            return await Task.FromResult(organizationFromContext);
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

