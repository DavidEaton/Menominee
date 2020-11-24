using Client.Models;
using System.Collections.Generic;
using System.Net.Http;
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
            var response = await httpClient.PostAsync(URISEGMENT, new StringContent(JsonSerializer.Serialize(person),
                                                                                    Encoding.UTF8, "application/json"));

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
            return await JsonSerializer.DeserializeAsync<IEnumerable<Person>>
                (await httpClient.GetStreamAsync(URISEGMENT), new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });
        }

        public async Task<Person> GetPerson(int id)
        {
            return await JsonSerializer.DeserializeAsync<Person>
                (await httpClient.GetStreamAsync($"persons/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdatePerson(Person person)
        {
            await httpClient.PutAsync("person", new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json"));
        }
    }
}
