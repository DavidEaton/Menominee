using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.SaleCodes
{
    public interface ISaleCodeRepository
    {
        Task AddSaleCodeAsync(SaleCode saleCode);
        Task<SaleCode> GetSaleCodeEntityAsync(string code);
        Task<SaleCode> GetSaleCodeEntityAsync(long id);
        Task<SaleCodeToRead> GetSaleCodeAsync(string code);
        Task<SaleCodeToRead> GetSaleCodeAsync(long id);
        Task<IReadOnlyList<SaleCodeToReadInList>> GetSaleCodeListAsync();
        void UpdateSaleCodeAsync(SaleCode saleCode);
        Task DeleteSaleCodeAsync(string code);
        Task<bool> SaleCodeExistsAsync(string code);
        Task<bool> SaveChangesAsync();
        void FixTrackingState();
    }
}
