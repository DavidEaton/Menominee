﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer entity);
        void DeleteCustomer(Customer entity);
        Task<bool> CustomerExistsAsync(long id);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<IReadOnlyList<CustomerToRead>> GetCustomersAsync();
        Task<IReadOnlyList<CustomerToReadInList>> GetCustomersInListAsync();
        Task<CustomerToRead> GetCustomerAsync(long id);
        Task SaveChangesAsync();
        Task<Customer> GetCustomerEntityAsync(long id);
    }
}
