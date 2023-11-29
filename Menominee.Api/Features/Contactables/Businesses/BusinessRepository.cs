using Menominee.Api.Data;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Contactables.Businesses
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly ApplicationDbContext context;

        public BusinessRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(Business business)
        {
            if (business is not null)
                context.Attach(business);
        }

        public void Delete(Business business)
        {
            context.Remove(business);
        }

        public async Task<BusinessToRead> GetAsync(long id)
        {
            var businessFromContext = await context.Businesses
                .Include(business => business.Phones)
                .Include(business => business.Emails)

                .Include(business => business.Contact)
                    .ThenInclude(contact => contact.Emails)
                .Include(business => business.Contact)
                    .ThenInclude(contact => contact.Phones)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(business => business.Id == id);

            return BusinessHelper.ConvertToReadDto(businessFromContext);
        }

        public async Task<IReadOnlyList<BusinessToRead>> GetAllAsync()
        {
            IReadOnlyList<Business> businessesFromContext = await context.Businesses
                .Include(business => business.Phones
                    .OrderByDescending(phone => phone.IsPrimary))

                .Include(business => business.Emails
                    .OrderByDescending(email => email.IsPrimary))

                .Include(business => business.Contact.Phones)
                .Include(business => business.Contact.Emails)

                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            return businessesFromContext
                .Select(business =>
                        BusinessHelper.ConvertToReadDto(business))
                .ToList();
        }

        public async Task<IReadOnlyList<BusinessToReadInList>> GetListAsync()
        {
            IReadOnlyList<Business> businessesFromContext = await context.Businesses
                .Include(business => business.Phones
                    .Where(phone => phone.IsPrimary == true))
                .Include(business => business.Emails
                    .Where(email => email.IsPrimary == true))

                .Include(business => business.Contact.Phones
                    .Where(phone => phone.IsPrimary == true))
                .Include(business => business.Contact.Emails
                    .Where(email => email.IsPrimary == true))

                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();

            return businessesFromContext
                .Select(business =>
                       BusinessHelper.ConvertToReadInListDto(business))
                .OrderBy(business => business.Name)
                .ToList();
        }

        public async Task<Business> GetEntityAsync(long id)
        {
            // Prefer FindAsync() over Single() or First() for single objects (non-collections);
            // FindAsync() checks the Identity Map Cache before making a trip to the database.
            var businessFromContext = context.Businesses
                .Include(business => business.Phones)
                .Include(business => business.Emails)
                .Include(business => business.Contact)
                    .ThenInclude(contact => contact.Emails)
                .Include(business => business.Contact)
                    .ThenInclude(contact => contact.Phones)
                .AsSplitQuery()
                .FirstOrDefaultAsync(business => business.Id == id);

            return await businessFromContext;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}