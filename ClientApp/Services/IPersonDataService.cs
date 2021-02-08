﻿using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    public interface IPersonDataService
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person> GetPerson(int id);
        Task<Person> AddPerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}