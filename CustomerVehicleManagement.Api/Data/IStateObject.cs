using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Api.Data
{
    public interface IStateTracker
    {
        /// <summary>
        /// IStateObject scheme moves entity state tracking out of the context and into the object itself to track entity state in disconnected applications.
        /// </summary>
        public TrackingState TrackingState { get; }
    }
}
