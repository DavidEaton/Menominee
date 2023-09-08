using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
                HandleException(dbContext, ex);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                HandleException(dbContext, ex);
            }
        }

        private static void HandleException(DbContext dbContext, DbUpdateException ex)
        {
            LogException(ex);
            ReloadEntries(ex.Entries);
            dbContext.SaveChanges();
        }

        private static void LogException(Exception ex)
        {
            var solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

            var logFilePath = Path.Combine(solutionDirectory, "TestLog.txt");

            File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.Message}\n");
        }

        private static void ReloadEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.Entity is not null)
                {
                    entry.Reload();
                }
            }
        }
    }
}
