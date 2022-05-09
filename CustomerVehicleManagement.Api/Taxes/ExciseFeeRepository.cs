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
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddExciseFeeAsync(ExciseFee exciseFee)
        {
            Guard.ForNull(exciseFee, "Excise Fee");

            if (await ExciseFeeExistsAsync(exciseFee.Id))
                throw new Exception("Excise Fee already exists");

            await context.AddAsync(exciseFee);
        }

        public async Task DeleteExciseFeeAsync(long id)
        {
            var exciseFee = await context.ExciseFees
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(fee => fee.Id == id);

            Guard.ForNull(exciseFee, "Excise Fee");

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

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<ExciseFee> UpdateExciseFeeAsync(ExciseFee exciseFee)
        {
            Guard.ForNull(exciseFee, "exciseFee");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(exciseFee).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExciseFeeExistsAsync(exciseFee.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
