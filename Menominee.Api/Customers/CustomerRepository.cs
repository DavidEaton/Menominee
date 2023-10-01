using Menominee.Api.Data;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
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

        public void Add(Customer customer)
        {
            if (customer is not null)
                context.Attach(customer);
        }

        public void Delete(Customer customer)
        {
            if (customer is not null)
                context.Remove(customer);
        }

        public async Task<CustomerToRead> GetAsync(long id)
        {
            return CustomerHelper.ConvertToReadDto(await GetEntityAsync(id));
        }

        public async Task<IReadOnlyList<CustomerToRead>> GetAllAsync()
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

        public async Task<PagedList<CustomerToRead>> GetByCodeAsync(string code, Pagination pagination)
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

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CustomerToReadInList>> GetListAsync()
        {
            var customersFromContext = await context.Customers
                // Person
                .Include(customer =>
                    customer.Person.Phones
                        .Where(phone => phone.IsPrimary))
                .Include(customer =>
                    customer.Person.Emails
                        .Where(email => email.IsPrimary))
                // Business and Business.Contact
                .Include(customer =>
                    customer.Business.Contact.Phones
                        .Where(phone => phone.IsPrimary))
                .Include(customer =>
                    customer.Business.Contact.Emails
                        .Where(email => email.IsPrimary))
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return customersFromContext
                .Select(customer => CustomerHelper.ConvertToReadInListDto(customer))
                .ToList();
        }

        public async Task<Customer> GetEntityAsync(long id)
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
