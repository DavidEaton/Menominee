using SharedKernel.Enums;

namespace SharedKernel.Interfaces
{
    public interface IEntity
    {
        int Id { get; }
        public TrackingState TrackingState { get; }
        public void UpdateState(TrackingState state);

    }
}
