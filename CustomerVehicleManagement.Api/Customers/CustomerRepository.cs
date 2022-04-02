using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models.Customers;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
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
                                                    // Person
                                                    .Include(customer =>
                                                             customer.Person.Phones)
                                                    .AsNoTracking()
                                                    .Include(customer =>
                                                             customer.Person.Emails)
                                                    .AsNoTracking()

                                                    // Organization and Organization.Contact
                                                    .Include(customer =>
                                                             customer.Organization.Contact.Phones)
                                                    .AsNoTracking()
                                                    .Include(customer => 
                                                             customer.Organization.Contact.Emails)
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(customer =>
                                                                         customer.Id == id);

            Guard.ForNull(customerFromContext, "customerFromContext");

            return CustomerHelper.ConvertToDto(customerFromContext);
        }

        public async Task<IReadOnlyList<CustomerToRead>> GetCustomersAsync()
        {
            var customers = new List<CustomerToRead>();

            var customersFromContext = await context.Customers
                                                    .Include(customer => customer.Person)
                                                    .AsNoTracking()
                                                    .Include(customer => customer.Organization)
                                                        .ThenInclude(organization => organization.Contact)
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            foreach (var customer in customersFromContext)
                customers.Add(CustomerHelper.ConvertToDto(customer));

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
                    return null;
                throw;
            }

            return null;
        }

        public async Task<IReadOnlyList<CustomerToReadInList>> GetCustomersInListAsync()
        {
            var customersFromContext = await context.Customers

                                                    // Person
                                                    .Include(customer =>
                                                             customer.Person.Phones
                                                             .Where(phone => phone.IsPrimary == true))
                                                    .AsNoTracking()
                                                    .Include(customer =>
                                                             customer.Person.Emails
                                                             .Where(email => email.IsPrimary == true))
                                                    .AsNoTracking()

                                                    // Organization and Organization.Contact
                                                    .Include(customer =>
                                                             customer.Organization.Contact.Phones
                                                             .Where(phone => phone.IsPrimary == true))
                                                    .AsNoTracking()
                                                    .Include(customer =>
                                                             customer.Organization.Contact.Emails
                                                             .Where(email => email.IsPrimary == true))
                                                    .AsNoTracking()
                                                    .ToArrayAsync();

            return customersFromContext
                .Select(customer => ConvertToDto(customer))
                .ToList();
        }

        private static CustomerToReadInList ConvertToDto(Customer customer)
        {
            if (customer != null)
            {
                if (customer.EntityType == EntityType.Person)
                {
                    return new CustomerToReadInList()
                    {
                        Id = customer.Id,
                        EntityType = customer.EntityType,
                        EntityId = customer.Person.Id,
                        CustomerType = customer.CustomerType.ToString(),
                        Name = customer.Person.Name.LastFirstMiddle,
                        AddressFull = customer.Person?.Address?.AddressFull is null ? string.Empty
                                                                                    : customer.Person?.Address.AddressFull,
                        PrimaryPhone = customer.Person?.Phones?.Count < 1           ? string.Empty
                                                                                    : customer.Person?.Phones[0]?.Number,
                        PrimaryEmail = customer.Person?.Emails?.Count < 1           ? string.Empty
                                                                                    : customer.Person?.Emails[0]?.Address
                    };
                }

                if (customer.EntityType == EntityType.Organization)
                {
                    return new CustomerToReadInList()
                    {
                        Id = customer.Id,
                        EntityType = customer.EntityType,
                        EntityId = customer.Organization.Id,
                        CustomerType = customer.CustomerType.ToString(),
                        Name = customer.Organization.Name.Name,
                        AddressFull = customer.Organization?.Address?.AddressFull is null ? string.Empty
                                                                                          : customer.Organization?.Address.AddressFull,
                        PrimaryPhone = customer.Organization?.Phones?.Count < 1           ? string.Empty
                                                                                          : customer.Organization?.Phones?[0]?.Number,
                        PrimaryEmail = customer.Organization?.Emails?.Count < 1           ? string.Empty
                                                                                          : customer.Organization?.Emails[0]?.Address
                    };
                }
            }

            return null;
        }

    }
}
