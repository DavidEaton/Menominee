using Menominee.Shared.Models.Company;
using System.Threading.Tasks;
using Entities = Menominee.Domain.Entities;

namespace Menominee.Api.Company
{
    public interface ICompanyRepository
    {
        void Add(Entities.Company entity);
        void Delete(Entities.Company entity);
        Task<Entities.Company> GetEntityAsync(long id);
        Task<CompanyToRead> GetAsync(long id);
        Task<long> GetNextInvoiceNumberOrSeedAsync();
    }
}
