using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class PersonDataService : IPersonDataService
    {
        private readonly HttpClient httpClient;
        private const string URISEGMENT = "persons";

        public PersonDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Person> AddPerson(Person person)
        {
            var content = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URISEGMENT, content);

            if (response.IsSuccessStatusCode)
                return await JsonSerializer.DeserializeAsync<Person>(await response.Content.ReadAsStreamAsync());

            return null;
        }

        public async Task DeletePerson(int id)
        {
            await httpClient.DeleteAsync($"person/{id}");
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            try
            {
                var persons = await httpClient.GetFromJsonAsync<Person[]>(httpClient.BaseAddress.ToString() + URISEGMENT);
                return persons;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return null;
        }

        public async Task<Person> GetPerson(int id)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<Person>(await httpClient.GetStreamAsync($"persons/{id}"), options);
        }

        public async Task UpdatePerson(Person person)
        {
            var content = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("person", content);
        }
    }
}
