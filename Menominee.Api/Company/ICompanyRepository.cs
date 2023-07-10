using Menominee.Shared.Models.Company;
using System.Threading.Tasks;
using Entities = Menominee.Domain.Entities;

namespace Menominee.Api.Company
{
    public interface ICompanyRepository
    {
        Task Add(Entities.Company entity);
        Task<Entities.Company> GetEntity(long id);
        Task<CompanyToRead> Get(long id);
        Task<long> GetNextInvoiceNumberOrSeed();
    }
}
