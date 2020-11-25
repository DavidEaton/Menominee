using Microsoft.EntityFrameworkCore;
using Migrations.Api.Data.Interfaces;
using Migrations.Core.Entities;
using SharedKernel.Enums;
using System;
using System.Threading.Tasks;

namespace Migrations.Api.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;
        public CustomerRepository()
        {
        }

        public CustomerRepository(AppDbContext context)
        {
            this.context = context;
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
            // Tracking is not needed (and expensive) for disconnected data collections
            var customer = await context.Customers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (customer.EntityType == EntityType.Organization)
                await GetOrganizationAddress(customer);

            if (customer.EntityType == EntityType.Person)
                await GetPersonAddress(customer);

            return customer;
        }

        private async Task GetOrganizationAddress(Customer customer)
        {
            customer.Entity = await context.Organizations.AsNoTracking()
                                                         .Include(o => o.Address)
                                                         .FirstOrDefaultAsync(o => o.Id == customer.EntityId);
            Organization entity = (Organization)customer.Entity;
            customer.Address = entity.Address;
        }

        private async Task GetPersonAddress(Customer customer)
        {
            customer.Entity = await context.Persons.AsNoTracking()
                                                   .Include(p => p.Address)
                                                   .FirstOrDefaultAsync(p => p.Id == customer.EntityId);
            Person entity = (Person)customer.Entity;
            customer.Address = entity.Address;
        }

        public async Task<Customer[]> GetCustomersAsync(bool includePhones = false)
        {
            var customers = await context.Customers.AsNoTracking().ToArrayAsync();

            foreach (var customer in customers)
            {
                if (customer.EntityType == EntityType.Organization)
                    await GetOrganizationAddress(customer);
                if (includePhones)
                    await GetOrganizationPhones(customer);

                if (customer.EntityType == EntityType.Person)
                    await GetPersonAddress(customer);
                if (includePhones)
                    await GetPersonPhones(customer);
            }

            return customers;
        }

        private Task GetPersonPhones(Customer customer)
        {
            throw new NotImplementedException();
        }

        private Task GetOrganizationPhones(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Customer customer)
        {
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
                throw new NullReferenceException("Customer Id missing.");

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
