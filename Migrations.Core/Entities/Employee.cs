using Migrations.Core.Interfaces;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migrations.Core.Entities
{
    public class Employee : IEntity, IEmployee
    {
        private DateTime? terminated;

        // EF requires an empty constructor
        protected Employee()
        {
        }

        public Employee(Person person, DateTime hired)
        {
            Person = person;
            Hired = hired;
            // Keep EF informed of object state in disconnected api
            TrackingState = TrackingState.Added;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Person Person { get; set; }

        // Enitity Framework convenience property
        public int PersonId { get; set; }
        public DateTime Hired { get; set; }

        public void Terminate(DateTime terminated)
        {
            SetTerminated(terminated);
        }

        public DateTime? GetTerminated()
        {
            return terminated;
        }

        private void SetTerminated(DateTime? value)
        {
            terminated = value;
        }

        [NotMapped]
        public bool Active { get => !GetTerminated().HasValue; }

        // EF State management for disconnected data
        public void UpdateState(TrackingState state)
        {
            TrackingState = state;
        }

        [NotMapped]
        public TrackingState TrackingState { get; private set; }

    }
}
