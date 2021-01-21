using Client.Models;
using Client.Services.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.ValueObjects;
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
        private PersonFlatDto moops;
        private const string URISEGMENT = "persons";

        public PersonDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<PersonFlatDto> AddPerson(PersonAddDto newPerson)
        {
            PersonName name = PersonUtilities.CreatePersonName(newPerson);

            if (name == null)
                return null;

            var personToAdd = new PersonAddDto(name, newPerson.Gender);

            var address = PersonUtilities.CreateAddress(newPerson);

            if (address != null)
                personToAdd.Address = address;

            var content = new StringContent(JsonSerializer.Serialize(personToAdd), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URISEGMENT, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<PersonFlatDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IEnumerable<PersonFlatDto>> GetAllPersons()
        {
            var persons = new List<PersonFlatDto>();

            try
            {
                var personsFromDatabase = await httpClient.GetFromJsonAsync<IEnumerable<PersonReadDto>>(URISEGMENT);

                return PersonUtilities.MapPersonsFromDatabaseToDto(persons, personsFromDatabase);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return null;
        }

        public async Task<PersonFlatDto> GetPersonDetails(int id)
        {
            var person = new PersonFlatDto();

            try
            {
                var personFromDatabase = await httpClient.GetFromJsonAsync<PersonReadDto>(URISEGMENT + $"/{id}");
                person = PersonUtilities.MapPersonFromDatabaseToDto(person, personFromDatabase);
                return person;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }

        public async Task UpdatePerson(PersonFlatDto person)
        {
            var content = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("person", content);
            //await httpClient.PutAsJsonAsync
        }

        public async Task DeletePerson(int id)
        {
            await httpClient.DeleteAsync($"person/{id}");
        }
    }
}
