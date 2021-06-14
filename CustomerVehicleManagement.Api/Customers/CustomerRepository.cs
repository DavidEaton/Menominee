using AutoMapper;
using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
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
                                                  .FirstOrDefaultAsync(customer => customer.Id == id);

            if (customer != null)
                return await MapCustomerToReadDto(customer);

            return null;
        }

        public async Task<IEnumerable<CustomerReadDto>> GetCustomersAsync()
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
                await GetCustomerOrganizationEntity(customer);
                customerReadDto.EntityId = ((Organization)customer.Entity).Id;
                customerReadDto.Address = ((Organization)customer.Entity).Address;
                customerReadDto.Name = ((Organization)customer.Entity).Name.Name;
                customerReadDto.Note = ((Organization)customer.Entity).Notes;
                MapPhones(((Organization)customer.Entity).Phones, customerReadDto.Phones);
                MapEmails(((Organization)customer.Entity).Emails, customerReadDto.Emails);
                if (((Organization)customer.Entity).Contact != null)
                    customerReadDto.Contact = MapContactToReadDto(((Organization)customer.Entity).Contact);
            }

            if (customer.EntityType == EntityType.Person)
            {
                await GetCustomerPersonEntity(customer);
                customerReadDto.EntityId = ((Person)customer.Entity).Id;
                customerReadDto.Address = ((Person)customer.Entity).Address;
                customerReadDto.Name = ((Person)customer.Entity).Name.LastFirstMiddle;
                MapPhones(((Person)customer.Entity).Phones, customerReadDto.Phones);
                MapEmails(((Person)customer.Entity).Emails, customerReadDto.Emails);
            }

            return customerReadDto;
        }

        public async Task GetCustomerPersonEntity(Customer customer)
        {
            var personFromContext = await context.Persons
                .Include(person => person.Phones)
                .Include(person => person.Emails)
                .FirstOrDefaultAsync(person => person.Id == customer.EntityId);

            if (personFromContext?.Phones != null)
                foreach (var phone in personFromContext.Phones)
                    customer.AddPhone(phone);

            if (personFromContext?.Emails != null)
                foreach (var email in personFromContext.Emails)
                    customer.AddEmail(email);

            customer.SetEntity(personFromContext);
        }

        public async Task GetCustomerOrganizationEntity(Customer customer)
        {
            var oganizationFromContext = await context.Organizations
                .Include(oganization => oganization.Phones)
                .Include(oganization => oganization.Emails)
                .Include(organization => organization.Contact)
                    .ThenInclude(contact => contact.Phones)
                .Include(organization => organization.Contact)
                    .ThenInclude(contact => contact.Emails)
                .FirstOrDefaultAsync(oganization => oganization.Id == customer.EntityId);

            if (oganizationFromContext?.Phones != null)
                foreach (var phone in oganizationFromContext.Phones)
                    customer.AddPhone(phone);

            if (oganizationFromContext?.Emails != null)
                foreach (var email in oganizationFromContext.Emails)
                    customer.AddEmail(email);

            customer.SetEntity(oganizationFromContext);
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
        private PersonReadDto MapContactToReadDto(Person contact)
        {
            PersonReadDto contactReadDto = new();

            contactReadDto.Id = contact.Id;
            contactReadDto.Address = contact.Address;
            contactReadDto.Birthday = contact.Birthday;
            contactReadDto.DriversLicense = contact.DriversLicense;
            contactReadDto.Gender = contact.Gender;
            contactReadDto.Name = contact.Name.LastFirstMiddle;
            contactReadDto.Emails = MapEmails(contact.Emails);
            contactReadDto.Phones = MapPhones(contact.Phones);

            return contactReadDto;
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

    }

}
