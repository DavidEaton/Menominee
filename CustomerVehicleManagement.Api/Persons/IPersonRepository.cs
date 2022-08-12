﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Persons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Persons
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person entity);
        void DeletePerson(Person entity);
        void FixTrackingState();
        Task<bool> PersonExistsAsync(long id);
        Task<IReadOnlyList<PersonToRead>> GetPersonsAsync();
        Task<IReadOnlyList<PersonToReadInList>> GetPersonsListAsync();
        Task<PersonToRead> GetPersonAsync(long id);
        Task<Person> GetPersonEntityAsync(long id);
        Task SaveChangesAsync();
    }
}