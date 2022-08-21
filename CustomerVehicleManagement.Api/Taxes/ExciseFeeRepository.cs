using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Taxes
{
    public class ExciseFeeRepository : IExciseFeeRepository
    {
        private readonly ApplicationDbContext context;

        public ExciseFeeRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddExciseFeeAsync(ExciseFee exciseFee)
        {
            // The Id of a new ExciseFee will never == an existing
            // Id because new domain objects don't get their Id
            // value until context.SaveChanges.
            //if (await ExciseFeeExistsAsync(exciseFee.Id))
            //    throw new Exception("Excise Fee already exists");

            if (exciseFee is not null)
                await context.AddAsync(exciseFee);

            await context.AddAsync(exciseFee);
        }

        public void DeleteExciseFee(ExciseFee exciseFee)
        {
            context.Remove(exciseFee);
        }

        public async Task<bool> ExciseFeeExistsAsync(long id)
        {
            return await context.ExciseFees.AnyAsync(fee => fee.Id == id);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<ExciseFeeToRead> GetExciseFeeAsync(long id)
        {
            var feeFromContext = await context.ExciseFees
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(fee => fee.Id == id);

            Guard.ForNull(feeFromContext, "Excise Fee");

            return ExciseFeeHelper.CreateExciseFee(feeFromContext);
        }

        public async Task<ExciseFee> GetExciseFeeEntityAsync(long id)
        {
            return await context.ExciseFees.FirstOrDefaultAsync(fee => fee.Id == id);
        }

        public async Task<IReadOnlyList<ExciseFeeToReadInList>> GetExciseFeeListAsync()
        {
            IReadOnlyList<ExciseFee> exciseFees = await context.ExciseFees.ToListAsync();

            return exciseFees
                .Select(fee => ExciseFeeHelper.CreateExciseFeeInList(fee))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
