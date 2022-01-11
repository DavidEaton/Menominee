using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Dtos.Customers;
using MenomineePlayWASM.Shared.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        //private readonly IHttpService httpService;
        //private string url = "api/customers";

        //public CustomerRepository(IHttpService httpService)
        //{
        //    this.httpService = httpService;
        //}

        //public async Task<int> CreateCustomer(Customer customer)
        //{
        //    var response = await httpService.Post<Customer, int>(url, customer);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<Customer> GetCustomer(int id)
        //{
        //    return await httpService.GetHelper<Customer>($"{url}/{id}");
        //}

        //public async Task<List<Customer>> GetCustomers()
        //{
        //    return await httpService.GetHelper<List<Customer>>(url);
        //}

        //public async Task<CustomerDto> GetCustomerDto(int id)
        //{
        //    return await httpService.GetHelper<CustomerDto>($"{url}/{id}");
        //}

        //public async Task UpdateCustomer(Customer customer)
        //{
        //    var response = await httpService.Put(url, customer);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
