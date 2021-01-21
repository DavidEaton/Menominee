using Client.Models;
using CustomerVehicleManagement.Domain.ValueObjects;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;

namespace Client.Services.Utilities
{
    public static class PersonUtilities
    {
        //public static PersonName CreatePersonName(PersonDto person)
        //{
        //    try
        //    {
        //        return new PersonName(person.LastName, person.FirstName, person.MiddleName);

        //    }
        //    catch (Exception)
        //    {
        //        //log it
        //    }

        //    return null;
        //}

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

        internal static Address CreateAddress(PersonAddDto person)
        {
            try
            {
                return new Address(person.AddressLine, person.City, person.State, person.PostalCode);
            }
            catch (Exception)
            {
                // log it
            }

            return null;
        }

        //internal static async Task<PersonGetDto> MapCreatedPersonToLoadDto(PersonAddDto personToAdd, HttpResponseMessage response)
        //{
        //    PersonGetDto created = await JsonSerializer.DeserializeAsync<PersonGetDto>(await response.Content.ReadAsStreamAsync());

        //    personToAdd.Id = created.Id;
        //    personToAdd.FirstName = created?.FirstName;
        //    personToAdd.MiddleName = created?.MiddleName;
        //    personToAdd.LastName = created?.LastName;
        //    personToAdd.AddressLine = created?.AddressLine;
        //    personToAdd.City = created?.AddressCity;
        //    personToAdd.State = created?.AddressState;
        //    personToAdd.PostalCode = created?.AddressPostalCode;
        //    personToAdd.Birthday = created?.Birthday;
        //    personToAdd.Gender = (Gender)created?.Gender;

        //    return created;
        //}

        internal static PersonName CreatePersonName(PersonAddDto person)
        {
            try
            {
                return new PersonName(person.Name.LastName, person.Name.FirstName, person.Name.MiddleName);

            }
            catch (Exception)
            {
                //log it
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