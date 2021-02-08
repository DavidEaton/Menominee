﻿using CustomerVehicleManagement.Domain.Entities;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer entity);
        void DeleteCustomer(Customer entity);
        void FixTrackingState();
        Task<bool> CustomerExistsAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<Customer[]> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<bool> SaveChangesAsync(Customer customer);
        Task<bool> SaveChangesAsync();
    }
}