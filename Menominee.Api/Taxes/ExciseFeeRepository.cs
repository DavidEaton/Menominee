using Menominee.Api.Data;
using Menominee.Domain.Entities.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Taxes
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
            // TODO:
            // The Id of a new ExciseFee will never == an existing
            // Id because new domain objects don't get their Id
            // value until context.SaveChanges.
            //if (await ExciseFeeExistsAsync(exciseFee.Id))
            //    throw new Exception("Excise Fee already exists");

            // TODO:
            // Is this a good use of "Use discard '_'"?
            // Is this a good use of exceptions?
            _ = exciseFee is not null
                ? await context.AddAsync(exciseFee)
                : throw new ArgumentNullException(nameof(context));
        }

        public void DeleteExciseFee(ExciseFee exciseFee)
        {
            context.Remove(exciseFee);
        }

        public async Task<bool> ExciseFeeExistsAsync(long id)
        {
            return await context.ExciseFees.AnyAsync(fee => fee.Id == id);
        }

        public async Task<ExciseFeeToRead> GetExciseFeeAsync(long id)
        {
            var feeFromContext = await context.ExciseFees
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(fee => fee.Id == id);

            return feeFromContext is not null
                ? ExciseFeeHelper.ConvertToReadDto(feeFromContext)
                : null;
        }

        public async Task<ExciseFee> GetExciseFeeEntityAsync(long id)
        {
            return await context.ExciseFees.FirstOrDefaultAsync(fee => fee.Id == id);
        }

        public async Task<IReadOnlyList<ExciseFeeToReadInList>> GetExciseFeeListAsync()
        {
            IReadOnlyList<ExciseFee> exciseFees = await context.ExciseFees.ToListAsync();

            return exciseFees
                .Select(fee => ExciseFeeHelper.ConvertToReadInListDto(fee))
                .ToList();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
