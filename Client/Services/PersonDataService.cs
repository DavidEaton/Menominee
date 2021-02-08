﻿using Client.Models;
using Client.Services.Utilities;
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
        public async Task<PersonFlatDto> AddPerson(PersonCreateDto newPerson)
        {
            var content = new StringContent(JsonSerializer.Serialize(newPerson), Encoding.UTF8, "application/json");
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
            PersonUpdateDto personToUpdate = PersonUtilities.MapUpdatedPersonToDto(person);
            var content = new StringContent(JsonSerializer.Serialize(personToUpdate), Encoding.UTF8, "application/json");

            try
            {
                await httpClient.PutAsync($"{URISEGMENT}/{personToUpdate.Id}", content);
            //await httpClient.PutAsJsonAsync
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
        }

        public async Task DeletePerson(int id)
        {
            await httpClient.DeleteAsync($"{URISEGMENT}/{id}");
        }
    }
}