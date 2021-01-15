using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class CustomerDataService
    {
        private readonly HttpClient httpClient;
        private const string URISEGMENT = "customers";

        public CustomerDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = await httpClient.GetFromJsonAsync<Customer[]>(httpClient.BaseAddress.ToString() + URISEGMENT);
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return null;
        }
    }
}
