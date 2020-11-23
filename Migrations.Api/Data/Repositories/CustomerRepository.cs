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
            var customer = await context.Customers.FirstOrDefaultAsync(p => p.Id == id);

            if (customer.EntityType == EntityType.Organization)
            {
                customer.Entity = await context.Organizations.Include(p => p.Address).FirstOrDefaultAsync(o => o.Id == customer.EntityId);
            }

            if (customer.EntityType == EntityType.Person)
            {
                customer.Entity = await context.Persons.Include(p => p.Address).FirstOrDefaultAsync(o => o.Id == customer.EntityId);
            }
                return customer;
        }

        public async Task<Customer[]> GetCustomersAsync()
        {
            return await context.Customers
                // Tracking is not needed (and expensive) for this disconnected data collection
                .AsNoTracking()
                //.Include(p => p.Address)
                .ToArrayAsync();
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
        }

        public async Task<bool> SaveChangesAsync(Customer customer)
        {
            // Mark customer EF object state = modified via dbContext:
            context.Customers
                .Update(customer);

            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void FixState()
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
