using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Menominee.Common.ValueObjects
{
    public abstract class AppValueObject : ValueObject
    {
        #region ORM
        public AppValueObject()
        {
            // Keep EF informed of object state in disconnected api
            TrackingState = TrackingState.Added;
        }

        // EF State management for disconnected data
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

        #endregion
    }
}
