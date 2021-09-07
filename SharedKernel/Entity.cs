using SharedKernel.Enums;

namespace SharedKernel
{
    public abstract class Entity
    {
        public virtual int Id { get; private set; }
        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        #region ORM

        public Entity()
        {
            // Keep EF informed of object state in disconnected api
            TrackingState = TrackingState.Added;
        }

        // EF State management for disconnected data
        public void SetTrackingState(TrackingState state)
        {
            TrackingState = state;
        }

        public TrackingState TrackingState { get; private set; }

        #endregion
    }
}
