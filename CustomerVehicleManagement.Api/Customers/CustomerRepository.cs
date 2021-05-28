using AutoMapper;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public CustomerRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Customer> AddAndSaveCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            if (customerCreateDto.PersonCreateDto != null)
            {
                var person = new Person(customerCreateDto.PersonCreateDto.Name, customerCreateDto.PersonCreateDto.Gender);
                // TODO: Add Birthday, DriversLicense, Address, Phones, Emails
                //...

                if (person != null)
                {
                    await context.AddAsync(person);
                    if (await SaveChangesAsync())
                    {
                        Customer customer = new(person);

                        if (customer != null)
                            await context.AddAsync(customer);

                        if (await SaveChangesAsync())
                            return customer;
                    }
                }
            }

            if (customerCreateDto.OrganizationCreateDto != null)
            {
                var organizationNameOrError = OrganizationName.Create(customerCreateDto.OrganizationCreateDto.Name);
                if (organizationNameOrError.IsFailure)
                    return null;

                var organization = new Organization(organizationNameOrError.Value);
                // TODO: Add Contact, Address, Phones, Emails
                //...

                if (organization != null)
                {
                    await context.AddAsync(organization);
                    if (await SaveChangesAsync())
                    {
                        Customer customer = new(organization);

                        if (customer != null)
                            await context.AddAsync(customer);

                        if (await SaveChangesAsync())
                            return customer;
                    }
                }
            }

            return null;
        }

        private async Task<IEntity> GetPersonEntityAsync(int entityId)
        {
            Person personFromContext = null;

            personFromContext = await context.Persons
                                                    .FirstOrDefaultAsync(person => person.Id == entityId);

            if (personFromContext != null)
                return personFromContext;

            return null;
        }

        private async Task<IEntity> GetOrganizationEntityAsync(int entityId)
        {
            Organization organizationFromContext = null;

            organizationFromContext = await context.Organizations
                                                                .FirstOrDefaultAsync(person => person.Id == entityId);

            if (organizationFromContext != null)
                return organizationFromContext;

            return null;
        }

        public void DeleteCustomer(Customer customer)
        {
            context.Remove(customer);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            var customer = await context.Customers.AsNoTracking()
                                                  .FirstOrDefaultAsync(customer => customer.Id == customer.Id);

            if (customer != null)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    var entity = await context.Organizations.FindAsync(customer.EntityId);

                    customer.SetEntity(entity);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    var entity = await context.Persons.FindAsync(customer.EntityId);

                    customer.SetEntity(entity);
                }

                return customer;
            }

            return null;
        }

        public async Task<IEnumerable<CustomerReadDto>> GetCustomersAsync()
        {
            var customers = new List<CustomerReadDto>();

            Customer[] customersRead = await context.Customers
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            foreach (var customer in customersRead)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    await GetCustomerOrganizationEntity(customer);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    await GetCustomerPersonEntity(customer);
                }

                customers.Add(customer.ToReadDto());
            }

            return customers;
        }

        public async Task GetCustomerPersonEntity(Customer customer)
        {
            var entity = await context.Persons.FindAsync(customer.EntityId);

            if (entity?.Phones != null)
                foreach (var phone in entity.Phones)
                    customer.AddPhone(phone);

            customer.SetEntity(entity);
        }

        public async Task GetCustomerOrganizationEntity(Customer customer)
        {
            var entity = await context.Organizations.FindAsync(customer.EntityId);

            if (entity?.Phones != null)
                foreach (var phone in entity.Phones)
                    customer.AddPhone(phone);

            if (entity?.Emails != null)
                foreach (var email in entity.Emails)
                    customer.AddEmail(email);

            customer.SetEntity(entity);
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Customer customer)
        {
            // Tracking IS needed for commands on disconnected data collections
            // Mark customer EF tracking state = modified via dbContext:
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
    }

}
