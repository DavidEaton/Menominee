﻿using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderTechEdit : ComponentBase
    {
        [Parameter]
        public RepairOrderServiceToWrite Service { get; set; }

        //[CascadingParameter]
        //public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public EventCallback OnDone { get; set; }

        private List<Technician> Technicians { get; set; } = new List<Technician>
        {
            new Technician { Id = 1, TechnicianId = 101, DisplayText = "101 - Bobby Brakedude" },
            new Technician { Id = 2, TechnicianId = 113, DisplayText = "113 - Sammy Shocker" },
            new Technician { Id = 3, TechnicianId = 205, DisplayText = "205 - Tony Tireman" },
            new Technician { Id = 4, TechnicianId = 385, DisplayText = "385 - Ed Exhauster" },
            new Technician { Id = 5, TechnicianId = 447, DisplayText = "447 - Alex Aligner" }
        };

        protected override void OnParametersSet()
        {
            if (Service?.Techs?.Count > 0)
            {
                foreach (var tech in Service.Techs)
                {
                    Technician technician = Technicians.Find(t => t.TechnicianId == tech.TechnicianId);
                    if (technician != null)
                    {
                        technician.Checked = true;
                    }
                }
            }
        }

        private void Save()
        {
            // Remove the techs that are no longer selected
            for (int i = Service.Techs.Count - 1; i >=0; i--)
            {
                Technician technician = Technicians.Find(t => (t.TechnicianId == Service.Techs[i].TechnicianId && t.Checked));
                if (technician == null)
                    Service.Techs.RemoveAt(i);
            }

            // Add the techs that weren't already selected
            foreach (var technician in Technicians)
            {
                if (technician.Checked)
                {
                    RepairOrderTechToWrite tech = Service.Techs.FirstOrDefault(t => t.TechnicianId == technician.TechnicianId);
                    if (tech == null)
                    {
                        Service.Techs.Add(new RepairOrderTechToWrite() { TechnicianId = technician.TechnicianId });
                    }
                }
            }

            OnDone.InvokeAsync();
        }

        private void Cancel()
        {
            OnDone.InvokeAsync();
        }

        public class Technician
        {
            public long Id { get; set; }
            public long TechnicianId { get; set; }
            public string DisplayText { get; set; }
            public bool Checked { get; set; } = false;
        }
    }
}