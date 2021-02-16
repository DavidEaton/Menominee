using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using CustomerVehicleManagement.Api.Data.Models;
using AutoMapper;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Repositories
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

        public void AddCustomer(Customer customer)
        {
            context.Add(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            context.Remove(customer);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            // Tracking is not needed (and expensive) for queries of disconnected data collections
            var customer = await context.Customers
                                        .AsNoTracking()
                                        .Include(c => c.Phones)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (customer.EntityType == EntityType.Organization)
            {
                var entity = await context.Organizations
                                               .AsNoTracking()
                                               .Include(o => o.Contact)
                                               .FirstOrDefaultAsync(o => o.Id == customer.EntityId);

                customer.SetEntity(entity);
            }

            if (customer.EntityType == EntityType.Person)
            {
                var entity = await context.Persons
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(p => p.Id == customer.EntityId);

                customer.SetEntity(entity);
            }

            return customer;
        }

        public async Task<IEnumerable<CustomerReadDto>> GetCustomersAsync()
        {
            var customers = new List<CustomerReadDto>();

            var customersRead = await context.Customers
                                         .AsNoTracking()
                                         .ToArrayAsync();

            foreach (var customer in customersRead)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    await ResolveOrganizationCustomer(customer);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    await ResolvePersonCustomer(customer);
                }

                customers.Add(customer.ToReadDto());
            }

            return customers;
        }

        private async Task ResolvePersonCustomer(Customer customer)
        {
            var entity = await context.Persons
                                           .AsNoTracking()
                                           .Include(person => person.Phones)
                                           .FirstOrDefaultAsync(person => person.Id == customer.EntityId);

            foreach (var phone in entity.Phones)
                customer.AddPhone(phone);

            customer.SetEntity(entity);
        }

        private async Task ResolveOrganizationCustomer(Customer customer)
        {
            var entity = await context.Organizations
                                      .AsNoTracking()
                                      .Include(organization => organization.Contact)
                                          .ThenInclude(contact => contact.Phones)
                                      .Include(organization => organization.Phones)
                                      .FirstOrDefaultAsync(o => o.Id == customer.EntityId);

            foreach (var phone in entity.Phones)
                customer.AddPhone(phone);
            
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
