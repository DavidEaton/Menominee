using Microsoft.EntityFrameworkCore;
using Migrations.Api.Data.Interfaces;
using SharedKernel.Enums;

namespace Migrations.Api.Data
{
    /// <summary>
    /// Moves entity state tracking back out of the object and into the context to track entity state in disconnected applications.
    /// </summary>
    public static class Utilities
    {
        private static EntityState ConvertState(TrackingState state)
        {
            switch (state)
            {
                case TrackingState.Added:
                    return EntityState.Added;
                case TrackingState.Modified:
                    return EntityState.Modified;
                case TrackingState.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        public static void FixState(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IStateObject>())
            {
                IStateObject stateInfo = entry.Entity;
                entry.State = ConvertState(stateInfo.ObjectState);
            }
        }

        //public static string GetTenantId(UserContext UserContext)
        //{
        //    var claims = UserContext.Claims;
        //    var tenantId = string.Empty;
        //    try
        //    {
        //        tenantId = claims.First(c => c.Type == "tenantId").Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Exception message from Utilities.GetTenantId(): {ex.Message}");
        //        return string.Empty;
        //    }

        //    return tenantId;
        //}

        //public static string GetTenantName(UserContext UserContext)
        //{
        //    var claims = UserContext.Claims;
        //    var tenantName = string.Empty;
        //    try
        //    {
        //        tenantName = claims.First(c => c.Type == "tenantName").Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Exception message from Utilities.GetTenantName(): {ex.Message}");
        //        return string.Empty;
        //    }

        //    return tenantName;
        //}
    }
}
