using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.SaleCodes
{
    public interface ISaleCodeDataService
    {
        Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodes();
        Task<SaleCodeToRead> GetSaleCode(long id);
        Task<SaleCodeToRead> AddSaleCode(SaleCodeToWrite saleCode);
        Task UpdateSaleCode(SaleCodeToWrite saleCode, long id);
    }
}
