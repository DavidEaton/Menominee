using CustomerVehicleManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.UiExperiments.Services
{
    public class PersonDataService : IPersonDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "persons";

        public PersonDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<PersonToRead> AddPerson(PersonToWrite newPerson)
        {
            var content = new StringContent(JsonSerializer.Serialize(newPerson), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<PersonToRead>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IReadOnlyList<PersonToRead>> GetAllPersons()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<PersonToRead>>(UriSegment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }

        public async Task<PersonToRead> GetPersonDetails(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<PersonToRead>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }

        public async Task UpdatePerson(PersonToWrite person)
        {

        }

        public async Task DeletePerson(long id)
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
