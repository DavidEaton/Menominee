using SharedKernel.Enums;

namespace SharedKernel.Interfaces
{
    public interface IEntity
    {
        int Id { get; }
        TrackingState TrackingState { get; }
        void UpdateTrackingState(TrackingState state);
        bool Equals(object obj);
        int GetHashCode();
    }
}
