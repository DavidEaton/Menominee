using Microsoft.EntityFrameworkCore;

namespace Menominee.Tests.Helpers
{
    public class DbContextHelper
    {
        public static void SaveChangesWithConcurrencyHandling(DbContext dbContext)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity != null)
                    {
                        entry.Reload();
                    }
                }

                dbContext.SaveChanges();
            }
        }
    }
}
