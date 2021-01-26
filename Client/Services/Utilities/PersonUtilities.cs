using Client.Models;
using CustomerVehicleManagement.Domain.ValueObjects;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using SharedKernel.Enums;

namespace Client.Services.Utilities
{
    public static class PersonUtilities
    {
        internal static Address CreateAddress(PersonFlatDto person)
        {
            try
            {
                return new Address(person.AddressLine, person.AddressCity, person.AddressState, person.AddressPostalCode);
            }
            catch (Exception)
            {
                // log it
            }

            return null;
        }

        internal static PersonFlatDto MapPersonFromDatabaseToDto(PersonFlatDto person, PersonReadDto personFromDatabase)
        {
            person.Id = personFromDatabase.Id;
            person.AddressLine = personFromDatabase?.Address?.AddressLine;
            person.AddressCity = personFromDatabase?.Address?.City;
            person.AddressState = personFromDatabase?.Address?.State;
            person.AddressPostalCode = personFromDatabase?.Address?.PostalCode;
            person.Birthday = personFromDatabase.Birthday;
            person.FirstName = personFromDatabase?.Name?.FirstName;
            person.MiddleName = personFromDatabase?.Name?.MiddleName;
            person.LastName = personFromDatabase?.Name?.LastName;
            person.Gender = personFromDatabase.Gender;

            return person;
        }

        internal static async Task<PersonFlatDto> MapCreatedPersonToDto(PersonFlatDto person, HttpResponseMessage response)
        {
            PersonFlatDto created = await JsonSerializer.DeserializeAsync<PersonFlatDto>(await response.Content.ReadAsStreamAsync());

            person.Id = created.Id;
            person.FirstName = created?.FirstName;
            person.MiddleName = created?.MiddleName;
            person.LastName = created?.LastName;
            person.AddressLine = created?.AddressLine;
            person.AddressCity = created?.AddressCity;
            person.AddressState = created?.AddressState;
            person.AddressPostalCode = created?.AddressPostalCode;
            person.Birthday = created?.Birthday;
            person.Gender = (Gender)created?.Gender;

            return person;
        }

        internal static IEnumerable<PersonFlatDto> MapPersonsFromDatabaseToDto(List<PersonFlatDto> persons, IEnumerable<PersonReadDto> personsFromDatabase)
        {
            foreach (var dbPerson in personsFromDatabase)
            {
                var person = new PersonFlatDto
                {
                    Id = dbPerson.Id,
                    AddressLine = dbPerson?.Address?.AddressLine,
                    AddressCity = dbPerson?.Address?.City,
                    AddressState = dbPerson?.Address?.State,
                    AddressPostalCode = dbPerson?.Address?.PostalCode,
                    Birthday = dbPerson.Birthday,
                    FirstName = dbPerson?.Name?.FirstName,
                    MiddleName = dbPerson?.Name?.MiddleName,
                    LastName = dbPerson?.Name?.LastName,
                    Gender = dbPerson.Gender
                };

                persons.Add(person);
            }

            return persons;
        }
    }
}