namespace Menominee.Common.Enums
{
    public enum TrackingState
    {
        /// <summary>
        /// Aligns with Entity Framework's EntityState enum
        /// </summary>
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}
