using Microsoft.EntityFrameworkCore;
using CustomerVehicleManagement.Api.Data.Interfaces;
using System;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;

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
            // Tracking is not needed (and expensive) for queries of disconnected data collections
            var customer = await context.Customers
                                        .AsNoTracking()
                                        .Include(c => c.Phones)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (customer.EntityType == EntityType.Organization)
            {
                customer.Entity = await context.Organizations
                                               .AsNoTracking()
                                               .Include(o => o.Contact)
                                               .FirstOrDefaultAsync(o => o.Id == customer.EntityId);
            }

            if (customer.EntityType == EntityType.Person)
            {
                customer.Entity = await context.Persons
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(p => p.Id == customer.EntityId);
            }

            return customer;
        }

        public async Task<Customer[]> GetCustomersAsync()
        {
            var customers = await context.Customers
                                         .AsNoTracking()
                                         .Include(c => c.Phones)
                                         .ToArrayAsync();

            foreach (var customer in customers)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    customer.Entity = await context.Organizations
                                                   .AsNoTracking()
                                                   .Include(o => o.Contact)
                                                   .FirstOrDefaultAsync(o => o.Id == customer.EntityId);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    customer.Entity = await context.Persons
                                                   .AsNoTracking()
                                                   .FirstOrDefaultAsync(p => p.Id == customer.EntityId);
                }
            }

            return customers;
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
