using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Api.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Menominee.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task DeleteCustomerAsync(long id)
        {
            var customer = await context.Customers.AsNoTracking()
                                      .FirstOrDefaultAsync(customer => customer.Id == id);

            Guard.ForNull(customer, "customer");

            context.Remove(customer);
        }

        public async Task<CustomerToRead> GetCustomerAsync(long id)
        {
            var customerFromContext = await context.Customers
                                        .Include(customer => customer.Person)
                                        .Include(customer => customer.Organization)
                                            .ThenInclude(organization => organization.Contact)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(customer => customer.Id == id);

            Guard.ForNull(customerFromContext, "customerFromContext");

            return CustomerToRead.ConvertToDto(customerFromContext);
        }

        public async Task<IReadOnlyList<CustomerToRead>> GetCustomersAsync()
        {
            var customers = new List<CustomerToRead>();

            var customersFromContext = await context.Customers
                                                    .Include(customer => customer.Person)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Contact)
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            foreach (var customer in customersFromContext)
                customers.Add(CustomerToRead.ConvertToDto(customer));

            return customers;
        }

        public async Task<bool> CustomerExistsAsync(long id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
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

        public async Task<IReadOnlyList<CustomerToReadInList>> GetCustomersInListAsync()
        {
            var customersFromContext = await context.Customers
                                                    .Include(customer => customer.Person)
                                                        .ThenInclude(person => person.Phones)
                                                    .Include(customer => customer.Person)
                                                        .ThenInclude(person => person.Emails)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Phones)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Emails)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Contact)
                                                            .ThenInclude(contact => contact.Phones)
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Contact)
                                                            .ThenInclude(contact => contact.Emails)
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            return customersFromContext
                .Select(customer =>
                        CustomerToReadInList.ConvertToDto(customer))
                .ToList();
        }
    }
}
