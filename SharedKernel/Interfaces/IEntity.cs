using SharedKernel.Enums;

namespace SharedKernel.Interfaces
{
    // VK: No need for an entity interface either
    
    public interface IEntity
    {
        int Id { get; }
        TrackingState TrackingState { get; }
        void SetTrackingState(TrackingState state);
        bool Equals(object obj);
        int GetHashCode();
    }
}
