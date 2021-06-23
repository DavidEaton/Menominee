using SharedKernel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SharedKernel.ValueObjects
{
    #region Comments
    // https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
    /// <summary>
    /// It could be argued that value objects, being immutable, should be read-only
    /// (that is, have get-only properties), and that's indeed true. However, value
    /// objects are usually serialized and deserialized to go through message queues,
    /// and being read-only stops the deserializer from assigning values, so we just
    /// leave them as private set, which is read-only enough to be practical.
    /// </summary>
    #endregion

    // VK: Regarding the comment above: I didn't see any mutable value objects in the code base, but still wanted to comment.
    // It's best to not make value objects mutable for the purpose of trasferring them over the wire
    // The same goes for the [JsonConstructor] attributes. What needs to be transferred between the server and its clients is DTOs
    // (data transfer objects), not value objects. Here's an article on the differences between the two: https://enterprisecraftsmanship.com/posts/dto-vs-value-object-vs-poco/
    // The reason for this guideline is
    // 1. Backward compatibility between clients and the server: you need to maintain it if the client is deployed separately from the server
    // 2. Separatation of concerns: DTOs represent data contracts between the server and the client; VOs model the domain. You don't want to conflate them.
    // In your case, looks like #1 is not a problem because the web client is deployed simulteniously with the server, but #2 still stands

    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        #region ORM
        public ValueObject()
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