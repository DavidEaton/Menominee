﻿using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.UiExperiments.Services
{
    public interface IPersonDataService
    {
        Task<IReadOnlyList<PersonReadDto>> GetAllPersons();
        Task<PersonReadDto> GetPersonDetails(int id);
        Task<PersonReadDto> AddPerson(PersonCreateDto person);
        Task UpdatePerson(PersonUpdateDto person);
        Task DeletePerson(int id);
    }
}