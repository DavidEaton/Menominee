using CustomerVehicleManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Api.Data
{
    /// <summary>
    /// Moves entity state tracking back out of the object and into the context to track entity state in disconnected applications.
    /// </summary>
    public static class EntityExtensions
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
            foreach (var entry in context.ChangeTracker.Entries<IStateTracker>())
            {
                IStateTracker state = entry.Entity;
                entry.State = ConvertState(state.TrackingState);
            }
        }
    }
}
