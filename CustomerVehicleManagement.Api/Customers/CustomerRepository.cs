using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;

        public CustomerRepository(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            Guard.ForNull(customer, "customer");

            if (await CustomerExistsAsync(customer.Id))
                throw new Exception("Customer already exists");


            await context.AddAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await context.Customers.AsNoTracking()
                                      .FirstOrDefaultAsync(customer => customer.Id == id);

            Guard.ForNull(customer, "customer");

            context.Remove(customer);
        }

        public async Task<CustomerReadDto> GetCustomerAsync(int id)
        {
            var customerFromContext = await context.Customers
                                        .Include(customer => customer.Person)
                                        .Include(customer => customer.Organization)
                                            .ThenInclude(organization => organization.Contact)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(customer => customer.Id == id);

            Guard.ForNull(customerFromContext, "customerFromContext");

            return CustomerToReadDto(customerFromContext);
        }

        public async Task<IReadOnlyList<CustomerReadDto>> GetCustomersAsync()
        {
            var customers = new List<CustomerReadDto>();

            var customersFromContext = await context.Customers
                                                    .Include(customer => customer.Person)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Contact)
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            foreach (var customer in customersFromContext)
                customers.Add(CustomerToReadDto(customer));

            return customers;
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

        public Task<IReadOnlyList<CustomerInListDto>> GetCustomersInListAsync()
        {
            throw new NotImplementedException();
        }

        private static CustomerReadDto CustomerToReadDto(Customer customer)
        {
            var customerReadDto = new CustomerReadDto
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType
            };

            if (customer.EntityType == EntityType.Organization)
            {
                customerReadDto.Organization = OrganizationToReadDto(customer.Organization);
                customerReadDto.Address = customerReadDto.Organization?.Address;
                customerReadDto.Name = customerReadDto.Organization.Name;
                customerReadDto.Note = customerReadDto.Organization?.Note;
                customerReadDto.Phones = customerReadDto.Organization?.Phones;
                customerReadDto.Emails = customerReadDto.Organization?.Emails;
                if (customer.Organization.Contact != null)
                    customerReadDto.Contact = PersonToReadDto(customer.Organization.Contact);
            }

            if (customer.EntityType == EntityType.Person)
            {
                customerReadDto.Person = PersonToReadDto(customer.Person);
                customerReadDto.Address = customerReadDto.Person?.Address;
                customerReadDto.Name = customerReadDto.Person.Name;
                customerReadDto.Phones = (IList<PhoneReadDto>)customerReadDto.Person?.Phones;
                customerReadDto.Emails = (IList<EmailReadDto>)customerReadDto.Person?.Emails;
            }

            if (customer.EntityType != EntityType.Person && customer.EntityType != EntityType.Organization)
                return null;

            return customerReadDto;
        }

        private static PersonReadDto PersonToReadDto(Person person)
        {
            PersonReadDto contactReadDto = new();

            contactReadDto.Id = person.Id;
            contactReadDto.Address = person?.Address;
            contactReadDto.Birthday = person?.Birthday;
            contactReadDto.DriversLicense = person?.DriversLicense;
            contactReadDto.Gender = person.Gender;
            contactReadDto.Name = person.Name.LastFirstMiddle;
            contactReadDto.Emails = EmailsToReadDto(person?.Emails);
            contactReadDto.Phones = PhonesToReadDto(person?.Phones);

            return contactReadDto;
        }

        private static OrganizationReadDto OrganizationToReadDto(Organization organization)
        {
            OrganizationReadDto organizationReadDto = new();

            organizationReadDto.Id = organization.Id;
            organizationReadDto.Address = organization.Address;
            organizationReadDto.Name = organization.Name.Name;
            organizationReadDto.Emails = (IList<EmailReadDto>)EmailsToReadDto(organization.Emails);
            organizationReadDto.Phones = (IList<PhoneReadDto>)PhonesToReadDto(organization.Phones);

            if (organizationReadDto.Contact != null)
                organizationReadDto.Contact = PersonToReadDto(organization.Contact);

            return organizationReadDto;
        }

        private static IReadOnlyList<PhoneReadDto> PhonesToReadDto(IList<Phone> phones)
        {
            List<PhoneReadDto> phoneReadDtos = new();

            if (phones != null)
                foreach (var phone in phones)
                    phoneReadDtos.Add(new PhoneReadDto() { Number = phone.Number, PhoneType = phone.PhoneType.ToString(), IsPrimary = phone.IsPrimary });

            return phoneReadDtos;
        }

        private static IReadOnlyList<EmailReadDto> EmailsToReadDto(IList<Email> emails)
        {
            List<EmailReadDto> emailReadDtos = new();

            if (emails != null)
                foreach (var email in emails)
                    emailReadDtos.Add(new EmailReadDto() { Address = email.Address, IsPrimary = email.IsPrimary });

            return emailReadDtos;
        }
    }

}
