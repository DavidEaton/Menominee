﻿using CustomerVehicleManagement.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Taxes
{
    public interface IExciseFeeDataService
    {
        Task<IReadOnlyList<ExciseFeeToReadInList>> GetAllExciseFeesAsync();
        Task<ExciseFeeToRead> GetExciseFeeAsync(long id);
        Task<ExciseFeeToRead> AddExciseFeeAsync(ExciseFeeToWrite exciseFee);
        Task UpdateExciseFeeAsync(long id, ExciseFeeToWrite exciseFee);
    }
}
