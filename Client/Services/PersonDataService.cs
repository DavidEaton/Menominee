using Client.Models;
using Client.Services.Utilities;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.ValueObjects;
using System;
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
        public async Task<PersonDto> AddPerson(PersonDto person)
        {
            PersonName name = PersonUtilities.CreatePersonName(person);

            if (name == null)
                return null;

            var personToAdd = new Person(name, person.Gender)
            {
                Birthday = person.Birthday
            };

            Address address = PersonUtilities.CreateAddress(person);

            if (address != null)
                person.AddAddress(address);

            DriversLicence driversLicence = PersonUtilities.CreateDriversLicense(person);

            if (driversLicence != null)
                person.AddDriversLicense(driversLicence);

            var content = new StringContent(JsonSerializer.Serialize(personToAdd), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URISEGMENT, content);

            if (response.IsSuccessStatusCode)
            {
                return await PersonUtilities.MapCreatedPersonToDto(person, response);
            }

            return null;
        }


        public async Task DeletePerson(int id)
        {
            await httpClient.DeleteAsync($"person/{id}");
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersons()
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }; 
            var persons = new List<PersonDto>();

            try
            {
                PersonLoadDto[] personsFromDatabase = await JsonSerializer.DeserializeAsync<PersonLoadDto[]>(await httpClient.GetStreamAsync(URISEGMENT), options);

                return PersonUtilities.MapPersonsFromDatabaseToDto(persons, personsFromDatabase);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            return null;
        }

        public async Task<PersonDto> GetPerson(int id)
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<PersonDto>(await httpClient.GetStreamAsync($"persons/{id}"), options);
        }

        public async Task UpdatePerson(PersonDto person)
        {
            var content = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
            await httpClient.PutAsync("person", content);
        }
    }
}
