using SharedKernel.ValueObjects;
using Client.Models;
using CustomerVehicleManagement.Domain.ValueObjects;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace Client.Services.Utilities
{
    public static class PersonUtilities
    {
        public static PersonName CreatePersonName(PersonDto person)
        {
            try
            {
                return new PersonName(person.LastName, person.FirstName, person.MiddleName);

            }
            catch (Exception)
            {
                //log it
            }

            return null;
        }

        internal static Address CreateAddress(PersonDto person)
        {
            try
            {
                return new Address(person.AddressLine, person.AddressCity, person.AddressState, person.AddressPostalCode, person.AddressCountryCode);
            }
            catch (Exception)
            {
                // log it
            }

            return null;
        }

        internal static DriversLicence CreateDriversLicense(PersonDto person)
        {
            DateTimeRange dateTimeRange = null;

            try
            {
                dateTimeRange = new DateTimeRange(person.DriversLicenceIssued, person.DriversLicenceExpiry);
            }
            catch (Exception)
            {
                // log it
            }

            if (dateTimeRange == null)
                return null;

            try
            {
                return new DriversLicence(person.DriversLicenceNumber, person.DriversLicenceState, dateTimeRange);
            }
            catch (Exception)
            {
                // log it
            }

            return null;
        }

        internal static async Task<PersonDto> MapCreatedPersonToDto(PersonDto person, HttpResponseMessage response)
        {
            PersonLoadDto created = await JsonSerializer.DeserializeAsync<PersonLoadDto>(await response.Content.ReadAsStreamAsync());

            person.Id = created.Id;
            person.FirstName = created?.Name?.FirstName;
            person.MiddleName = created?.Name?.MiddleName;
            person.LastName = created?.Name?.LastName;
            person.AddressLine = created?.Address?.AddressLine;
            person.AddressCity = created?.Address?.City;
            person.AddressState = created?.Address?.State;
            person.AddressPostalCode = created?.Address?.PostalCode;
            person.AddressCountryCode = created?.Address?.CountryCode;
            person.DriversLicenceNumber = created?.DriversLicence?.Number;
            person.DriversLicenceState = created?.DriversLicence?.State;
            //person.DriversLicenceIssued = created.DriversLicence.ValidRange.Start;
            //person.DriversLicenceExpiry = created.DriversLicence.ValidRange.End;
            person.Birthday = created?.Birthday;
            person.Gender = (CustomerVehicleManagement.Domain.Enums.Gender)created?.Gender;

            return person;
        }

        internal static IEnumerable<PersonDto> MapPersonsFromDatabaseToDto(List<PersonDto> persons, PersonLoadDto[] personsFromDatabase)
        {
            foreach (var dbPerson in personsFromDatabase)
            {
                var person = new PersonDto
                {
                    Id = dbPerson.Id,
                    AddressLine = dbPerson?.Address?.AddressLine,
                    AddressCity = dbPerson?.Address?.City,
                    AddressState = dbPerson?.Address?.State,
                    AddressPostalCode = dbPerson?.Address?.PostalCode,
                    AddressCountryCode = dbPerson?.Address?.CountryCode,
                    Birthday = dbPerson.Birthday,
                    DriversLicenceNumber = dbPerson?.DriversLicence?.Number,
                    DriversLicenceState = dbPerson?.DriversLicence?.State,
                    //DriversLicenceIssued = (DateTime)(dbPerson?.DriversLicence?.ValidRange?.Start),
                    //DriversLicenceExpiry = (DateTime)(dbPerson?.DriversLicence?.ValidRange?.End),
                    FirstName = dbPerson?.Name?.FirstName,
                    MiddleName = dbPerson?.Name?.MiddleName,
                    LastName = dbPerson?.Name?.LastName,
                    Gender = (CustomerVehicleManagement.Domain.Enums.Gender)dbPerson.Gender
                };

                persons.Add(person);
            }

            return persons;
        }

    }

}
