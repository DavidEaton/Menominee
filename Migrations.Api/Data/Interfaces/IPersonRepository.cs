using Migrations.Core.Entities;
using System.Threading.Tasks;

namespace Migrations.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        void AddPerson(Person entity);
        void DeletePerson(Person entity);
        void FixState();
        Task<bool> PersonExistsAsync(int id);
        Task<Person> UpdatePersonAsync(Person entity);
        Task<Person[]> GetPersonsAsync();
        Task<Person> GetPersonAsync(int id);
        Task<bool> SaveChangesAsync(Person person);
        Task<bool> SaveChangesAsync();
    }
}
