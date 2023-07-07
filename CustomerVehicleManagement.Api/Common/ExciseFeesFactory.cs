using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomerVehicleManagement.Api.Common
{
    public static class ExciseFeesFactory
    {
        public static List<ExciseFee> Create(IReadOnlyList<ExciseFeeToWrite> exciseFeesToWrite)
        {
            return (exciseFeesToWrite ?? new List<ExciseFeeToWrite>())
            .Select(fee =>
            {
                var createdExciseFee = ExciseFee.Create(fee.Description, fee.FeeType, fee.Amount).Value;
                typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).SetValue(createdExciseFee, fee.Id);
                return createdExciseFee;
            })
            .ToList();
        }
    }
}
