using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using CustomerVehicleManagement.Api.Data.Models;
using AutoMapper;
using System.Collections.Generic;
using CustomerVehicleManagement.Api.Utilities;

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
            var customer = context.Customers.Find(id);

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
                    await MapOrganizationCustomer(customer);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    await MapPersonCustomer(customer);
                }

                customers.Add(customer.ToReadDto());
            }

            return customers;
        }

        private async Task MapPersonCustomer(Customer customer)
        {
            var entity = await context.Persons.FindAsync(customer.EntityId);

            foreach (var phone in entity.Phones)
                customer.AddPhone(phone);

            customer.SetEntity(entity);
        }

        private async Task MapOrganizationCustomer(Customer customer)
        {
            var entity = await context.Organizations.FindAsync(customer.EntityId);

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
