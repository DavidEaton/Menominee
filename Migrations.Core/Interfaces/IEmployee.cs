using Migrations.Core.Entities;
using System;

namespace Migrations.Core.Interfaces
{
    public interface IEmployee
    {
        bool Active { get; }
        DateTime Hired { get; set; }
        Person Person { get; set; }
        int PersonId { get; set; }
        DateTime? GetTerminated();
        void Terminate(DateTime terminated);
    }
}