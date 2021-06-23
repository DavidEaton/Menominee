using CustomerVehicleManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly HttpClient httpClient;
        private const string MediaType = "application/json";
        private const string UriSegment = "customers";

        public CustomerDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<CustomerReadDto> AddCustomer(CustomerCreateDto newCustomer)
        {
            var content = new StringContent(JsonSerializer.Serialize(newCustomer), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<CustomerReadDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IReadOnlyList<CustomerInListDto>> GetAllCustomers()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<CustomerInListDto>>($"{UriSegment}/list");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }

        public async Task<CustomerReadDto> GetCustomerDetails(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<CustomerReadDto>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }

        //public async Task UpdateCustomer(CustomerUpdateDto person)
        //{
            //CustomerUpdateDto personToUpdate = CustomerUtilities.MapUpdatedCustomerToDto(person);
            //var content = new StringContent(JsonSerializer.Serialize(personToUpdate), Encoding.UTF8, "application/json");

            //try
            //{
            //    await httpClient.PutAsync($"{UriSegment}/{personToUpdate.Id}", content);
            //    //await httpClient.PutAsJsonAsync
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Message :{ex.Message}");
            //}
        //}

        public async Task DeleteCustomer(int id)
        {
            try
            {
                await httpClient.DeleteAsync($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
        }
    }
}