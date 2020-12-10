using SharedKernel.Enums;
using System;

namespace Migrations.Core.Entities
{
    public interface IEmployee
    {
        bool Active { get; }
        DateTime Hired { get; set; }
        int Id { get; set; }
        Person Person { get; set; }
        int PersonId { get; set; }
        TrackingState TrackingState { get; }

        DateTime? GetTerminated();
        void Terminate(DateTime terminated);
        void UpdateState(TrackingState state);
    }
}