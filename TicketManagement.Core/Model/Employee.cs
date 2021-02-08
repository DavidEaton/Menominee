﻿using System;
using TicketManagement.Core.Entities;

namespace TicketManagement.Core.Model
{
    public class Employee
    {
        public int Id { get; }
        public Person Person { get; }

        // Enitity Framework convenience property
        public int PersonId { get; }
        public DateTime Hired { get; }
    }
}