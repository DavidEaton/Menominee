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

namespace Menominee.Api.Features.Customers
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
            Helper.SetShadowProperties(context, customer);
        }

        public void Delete(Customer customer)
        {
            if (customer is not null)
                context.Remove(customer);
        }

        public async Task<CustomerToRead?> GetAsync(long id)
        {
            var result = await GetEntityAsync(id);

            return result is null
                ? null
                : CustomerHelper.ConvertToReadDto(result);
        }

        public async Task<IReadOnlyList<CustomerToRead?>> GetAllAsync()
        {
            return await GetCustomersAsync<CustomerToRead?>(CustomerHelper.ConvertToReadDto);
        }

        public async Task<Customer?> GetEntityAsync(long id)
        {
            var customer = context.Customers
                .Where(customer => customer.Id == id)
                .FirstOrDefault();

            if (customer is null)
            {
                return null;
            }

            await Helper.LoadRelatedDataAsync(customer, context);

            return customer;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CustomerToReadInList?>> GetListAsync()
        {
            return await GetCustomersAsync<CustomerToReadInList>(CustomerHelper.ConvertToReadInListDto);
        }

        private async Task<IReadOnlyList<T>> GetCustomersAsync<T>(Func<Customer?, T> convertToDto)
        {
            var customers = new List<T>();
            var customersFromContext = await context.Customers
                .AsSplitQuery()
                .ToArrayAsync();

            foreach (var customer in customersFromContext)
            {
                await Helper.LoadRelatedDataAsync(customer, context);
                customers.Add(convertToDto(customer));
            }

            return customers;
        }

        public async Task<PagedList<CustomerToRead>> GetByCodeAsync(string code, Pagination pagination)
        {
            var query = await GetAllAsync();

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

    }
}
