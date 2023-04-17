using CustomerVehicleManagement.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerVehicleManagement.Tests.Integration.Data
{
    public static class DbContextExtensions
    {
        public static string GetTableName<T>(this ApplicationDbContext dbContext) where T : class
        {
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            var schema = entityType.GetSchema() ?? dbContext.Model.GetDefaultSchema();
            var tableName = entityType.GetTableName();
            return $"[{schema}].[{tableName}]";
        }
    }
}
