using CustomerVehicleManagement.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Taxes
{
    public interface ISalesTaxDataService
    {
        Task<IReadOnlyList<SalesTaxToReadInList>> GetAllSalesTaxesAsync();
        Task<SalesTaxToRead> GetSalesTaxAsync(long id);
        Task<SalesTaxToRead> AddSalesTaxAsync(SalesTaxToWrite salesTax);
        Task UpdateSalesTaxAsync(long id, SalesTaxToWrite salesTax);
    }
}
