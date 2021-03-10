using AutoMapper;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Data.Services
{
    public class PersonService
    {
        private readonly IPersonRepository repository;
        private readonly IMapper mapper;

        public PersonService(IPersonRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PersonReadDto> GetPersonAsync(int id)
        {
            var personFromContext = await repository.GetPersonAsync(id);

            return mapper.Map<PersonReadDto>(personFromContext);
        }

        public async Task<IEnumerable<PersonInListDto>> GetPersonsListAsync()
        {
            return await repository.GetPersonsListAsync();

        }
    }
}
