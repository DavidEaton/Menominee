using AutoMapper;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;

        public CustomerRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer != null)
                await context.AddAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await context.Customers.AsNoTracking()
                                      .FirstOrDefaultAsync(customer => customer.Id == id);

            if (customer != null)
                context.Remove(customer);
        }

        public async Task<CustomerReadDto> GetCustomerAsync(int id)
        {
            var customer = await context.Customers.AsNoTracking()
                                                  .Include(x => x.Organization)
                                                  .Include(x => x.Person)
                                                  .FirstOrDefaultAsync(customer => customer.Id == id);

            if (customer == null)
                return null;

            var customerReadDto = new CustomerReadDto
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType
            };

            if (customer.EntityType == EntityType.Organization)
            {
                Organization customerOrganization = await GetCustomerOrganization(customer);
                customerReadDto.Organization = MapOrganizationToReadDto(customerOrganization);
                customerReadDto.Address = customerOrganization?.Address;
                customerReadDto.Name = customerOrganization.Name.Name;
                customerReadDto.Note = customerOrganization.Note;
                MapPhones(customerOrganization.Phones, customerReadDto.Phones);
                MapEmails(customerOrganization.Emails, customerReadDto.Emails);
                if (customerOrganization.Contact != null)
                    customerReadDto.Contact = MapPersonToReadDto(customerOrganization.Contact);
            }

            if (customer.EntityType == EntityType.Person)
            {
                Person customerPerson = await GetCustomerPerson(customer);
                customerReadDto.Person = MapPersonToReadDto(customerPerson);
                customerReadDto.Address = customerPerson.Address;
                customerReadDto.Name = customerPerson.Name.LastFirstMiddle;
                MapPhones(customerPerson.Phones, customerReadDto.Phones);
                MapEmails(customerPerson.Emails, customerReadDto.Emails);
            }


            return customerReadDto;
        }

        //public async Task<IReadOnlyList<CustomerInListDto>> GetCustomersInListAsync()
        //{
        //    var customersFromContext = await context.Customers.ToArrayAsync();

        //    var customersList = new List<CustomerInListDto>();

        //    foreach (Customer customer in customersFromContext)
        //        customersList.Add(new CustomerInListDto() { Id = customer.Id,
        //                                                    Name = customer.EntityType == EntityType.Organization
        //                                                                                ? customer.Organization.Name.Name
        //                                                                                : customer.Person.Name.LastFirstMiddle,
        //                                                    EntityId = customer.EntityType == EntityType.Organization
        //                                                                                ? customer.Organization.Id
        //                                                                                : customer.Person.Id,
        //                                                    EntityType = customer.EntityType,
        //                                                    CustomerType = customer.CustomerType,
        //                                                    AddressFull = customer.EntityType == EntityType.Organization
        //                                                                                ? customer.Organization.Address.AddressFull
        //                                                                                : customer.Person.Address.AddressFull,
        //                                                    Note = customer.EntityType == EntityType.Organization
        //                                                                                ? customer.Organization.Note
        //                                                                                : string.Empty,
        //                                                    PrimaryPhone = customer.EntityType == EntityType.Organization
        //                                                                                ? GetPrimaryPhone(customer.Organization.Phones)
        //                                                                                : GetPrimaryPhone(customer.Person.Phones),
        //        });



        //    return customersList;
        //}

        //private string GetPrimaryPhone(IList<Phone> phones)
        //{
        //    if (phones == null)
        //        return null;

        //    var phone = phones.SingleOrDefault(x => x.IsPrimary == true);

        //    if (phone == null)
        //        return phones[0].Number;

        //}

        public async Task<IReadOnlyList<CustomerReadDto>> GetCustomersAsync()
        {
            var customers = new List<CustomerReadDto>();

            Customer[] customersRead = await context.Customers
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            foreach (var customer in customersRead)
                customers.Add(await MapCustomerToReadDto(customer));

            return customers;
        }

        private async Task<CustomerReadDto> MapCustomerToReadDto(Customer customer)
        {
            var customerReadDto = new CustomerReadDto
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType
            };

            if (customer.EntityType == EntityType.Organization)
            {
                Organization customerOrganization = await GetCustomerOrganization(customer);
                customerReadDto.Organization = MapOrganizationToReadDto(customerOrganization);
                customerReadDto.Address = customerOrganization?.Address;
                customerReadDto.Name = customerOrganization.Name.Name;
                customerReadDto.Note = customerOrganization.Note;
                MapPhones(customerOrganization.Phones, customerReadDto.Phones);
                MapEmails(customerOrganization.Emails, customerReadDto.Emails);
                if (customerOrganization.Contact != null)
                    customerReadDto.Contact = MapPersonToReadDto(customerOrganization.Contact);
            }

            if (customer.EntityType == EntityType.Person)
            {
                Person customerPerson = await GetCustomerPerson(customer);
                customerReadDto.Person = MapPersonToReadDto(customerPerson);
                customerReadDto.Address = customerPerson.Address;
                customerReadDto.Name = customerPerson.Name.LastFirstMiddle;
                MapPhones(customerPerson.Phones, customerReadDto.Phones);
                MapEmails(customerPerson.Emails, customerReadDto.Emails);
            }

            if (customer.EntityType != EntityType.Person && customer.EntityType != EntityType.Organization)
                return null;

                return customerReadDto;
        }

        private async Task<Person> GetCustomerPerson(Customer customer)
        {
            var personFromContext = await context.Persons
                .Include(person => person.Phones)
                .Include(person => person.Emails)
                .FirstOrDefaultAsync(person => person.Id == customer.Person.Id);

            return personFromContext;

        }

        private async Task<Organization> GetCustomerOrganization(Customer customer)
        {
            var organizationFromContext = await context.Organizations
                .Include(organization => organization.Phones)
                .Include(organization => organization.Emails)
                .Include(organization => organization.Contact)
                    .ThenInclude(c => c.Phones)
                .Include(organization => organization.Contact)
                    .ThenInclude(c => c.Emails)
                .FirstOrDefaultAsync(organization => organization.Id == customer.Organization.Id);

            return organizationFromContext;

        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Customer customer)
        {
            context.Customers
                .Update(customer);

            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new NullReferenceException("Customer is missing.");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(customer).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExistsAsync(customer.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
        private PersonReadDto MapPersonToReadDto(Person person)
        {
            PersonReadDto contactReadDto = new();

            contactReadDto.Id = person.Id;
            contactReadDto.Address = person.Address;
            contactReadDto.Birthday = person.Birthday;
            contactReadDto.DriversLicense = person.DriversLicense;
            contactReadDto.Gender = person.Gender;
            contactReadDto.Name = person.Name.LastFirstMiddle;
            contactReadDto.Emails = MapEmails(person.Emails);
            contactReadDto.Phones = MapPhones(person.Phones);

            return contactReadDto;
        }

        private OrganizationReadDto MapOrganizationToReadDto(Organization organization)
        {
            OrganizationReadDto organizationReadDto = new();

            organizationReadDto.Id = organization.Id;
            organizationReadDto.Address = organization.Address;
            organizationReadDto.Name = organization.Name.Name;
            organizationReadDto.Emails = MapEmails(organization.Emails);
            organizationReadDto.Phones = MapPhones(organization.Phones);

            if (organizationReadDto.Contact != null)
            organizationReadDto.Contact = MapPersonToReadDto(organization.Contact);

            return organizationReadDto;
        }

        private IReadOnlyList<PhoneReadDto> MapPhones(IList<Phone> phones)
        {
            List<PhoneReadDto> phoneReadDtos = new();

            if (phones != null)
                foreach (var phone in phones)
                    phoneReadDtos.Add(new PhoneReadDto() { Number = phone.Number, PhoneType = phone.PhoneType.ToString(), IsPrimary = phone.IsPrimary });

            return phoneReadDtos;
        }

        private IReadOnlyList<EmailReadDto> MapEmails(IList<Email> emails)
        {
            List<EmailReadDto> emailReadDtos = new();

            if (emails != null)
                foreach (var email in emails)
                    emailReadDtos.Add(new EmailReadDto() { Address = email.Address, IsPrimary = email.IsPrimary });

            return emailReadDtos;
        }

        private static void MapPhones(IList<Phone> phones, IList<PhoneReadDto> phoneReadDtos)
        {
            if (phones != null)
                foreach (var phone in phones)
                    phoneReadDtos.Add(new PhoneReadDto() { Number = phone.Number, PhoneType = phone.PhoneType.ToString(), IsPrimary = phone.IsPrimary });
        }

        private static void MapEmails(IList<Email> emails, IList<EmailReadDto> emailReadDtos)
        {
            if (emails != null)
                foreach (var email in emails)
                    emailReadDtos.Add(new EmailReadDto() { Address = email.Address, IsPrimary = email.IsPrimary });
        }

        public Task<IReadOnlyList<CustomerInListDto>> GetCustomersInListAsync()
        {
            throw new NotImplementedException();
        }
    }

}
