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

        public void Add(ExciseFee exciseFee)
        {
            if (exciseFee is not null)
                context.Attach(exciseFee);
        }

        public void Delete(ExciseFee exciseFee)
        {
            context.Remove(exciseFee);
        }

        public async Task<ExciseFeeToRead> GetAsync(long id)
        {
            var feeFromContext = await context.ExciseFees
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(fee => fee.Id == id);

            return feeFromContext is not null
                ? ExciseFeeHelper.ConvertToReadDto(feeFromContext)
                : null;
        }

        public async Task<ExciseFee> GetEntityAsync(long id)
        {
            return await context.ExciseFees.FirstOrDefaultAsync(fee => fee.Id == id);
        }

        public async Task<IReadOnlyList<ExciseFeeToReadInList>> GetListAsync()
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
