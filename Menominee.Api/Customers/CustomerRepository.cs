using Menominee.Api.Data;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer is null)
                throw new ArgumentOutOfRangeException(nameof(customer), "customer");

            if (await CustomerExistsAsync(customer.Id))
                throw new Exception("Customer already exists");

            await context.AddAsync(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            context.Remove(customer);
        }

        public async Task<CustomerToRead> GetCustomerAsync(long id)
        {
            return CustomerHelper.ConvertToReadDto(await GetCustomerEntityAsync(id));
        }

        public async Task<IReadOnlyList<CustomerToRead>> GetCustomersAsync()
        {
            var customers = new List<CustomerToRead>();

            var customersFromContext = await context.Customers
                .Include(customer => customer.Person)
                .Include(customer => customer.Business)
                    .ThenInclude(business => business.Contact)
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            foreach (var customer in customersFromContext)
                customers.Add(CustomerHelper.ConvertToReadDto(customer));

            return customers;
        }

        public async Task<PagedList<CustomerToRead>> GetCustomersAsync(string code, Pagination pagination)
        {
            var query = await context.Customers
                .Where(customer => customer.Code.Equals(code))
                .Include(customer => customer.Person)
                .Include(customer => customer.Business)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(customer => CustomerHelper.ConvertToReadDto(customer))
                .ToListAsync();

            var orderedQuery = pagination.SortOrder.Equals(SortOrder.Desc)
                ? query.OrderByDescending(GetSortProperty(pagination.SortColumn)).AsQueryable()
                : query.OrderBy(GetSortProperty(pagination.SortColumn)).AsQueryable();

            return PagedList<CustomerToRead>.Create(
                orderedQuery,
                pagination.PageNumber,
                pagination.PageSize);
        }

        private static Func<CustomerToRead, string> GetSortProperty(VehicleSortColumn sortColumn)
        {
            return sortColumn switch
            {
                _ => customer => customer.Name
            };
        }

        public async Task<bool> CustomerExistsAsync(long id)
        {
            return await context.Customers
                .AnyAsync(customer => customer.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
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
                .Include(customer =>
                    customer.Person.Emails
                        .Where(email => email.IsPrimary == true))
                // Business and Business.Contact
                .Include(customer =>
                    customer.Business.Contact.Phones
                        .Where(phone => phone.IsPrimary == true))
                .Include(customer =>
                    customer.Business.Contact.Emails
                        .Where(email => email.IsPrimary == true))
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return customersFromContext
                .Select(customer => ConvertToReadInListDto(customer))
                .ToList();
        }

        private static CustomerToReadInList ConvertToReadInListDto(Customer customer)
        {
            if (customer is not null)
            {
                return new CustomerToReadInList()
                {
                    Id = customer.Id,
                    EntityType = customer.EntityType,
                    CustomerType = customer.CustomerType,
                    Name = customer.Name,
                    AddressFull = customer?.Address?.AddressFull,
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(customer?.Contact),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(customer?.Contact)
                };
            }
            return null;
        }

        public async Task<Customer> GetCustomerEntityAsync(long id)
        {
            var customerFromContext = await context.Customers
                // Person
                .Include(customer =>
                    customer.Person.Phones)
                .Include(customer =>
                    customer.Person.Emails)
                // Business and Business.Contact
                .Include(customer =>
                    customer.Business.Contact.Phones)
                .Include(customer =>
                    customer.Business.Contact.Emails)
                // Vehicles
                .Include(customer =>
                    customer.Vehicles)

                .AsSplitQuery()
                .FirstOrDefaultAsync(customer => customer.Id == id);

            return customerFromContext;
        }
    }
}
