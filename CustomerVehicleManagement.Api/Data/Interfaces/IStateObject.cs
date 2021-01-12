using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Interfaces
{
    public interface IStateObject
    {
        /// <summary>
        /// IStateObject scheme moves entity state tracking out of the context and into the object itself to track entity state in disconnected applications.
        /// </summary>
        public TrackingState TrackingState { get; }
    }
}
