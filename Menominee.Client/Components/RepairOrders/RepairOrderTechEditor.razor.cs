using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderTechEditor : ComponentBase
    {
        [Parameter]
        public RepairOrderServiceToWrite Service { get; set; }

        //[CascadingParameter]
        //public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public EventCallback OnDone { get; set; }

        private List<RepairOrderServiceTechnicianToRead> Technicians { get; set; } = new List<RepairOrderServiceTechnicianToRead>
        {
            //new TechnicianSelector { Id = 9, TechnicianId = 101, DisplayText = "101 - Bobby Brakedude" },
            //new RepairOrderServiceTechnicianToRead { Id = 1, new EmploymentToRead { Id = 1, Employee =  } },
            //new TechnicianSelector { Id = 2, TechnicianId = 113, DisplayText = "113 - Sammy Shocker" },
            //new TechnicianSelector { Id = 3, TechnicianId = 205, DisplayText = "205 - Tony Tireman" },
            //new TechnicianSelector { Id = 4, TechnicianId = 385, DisplayText = "385 - Ed Exhauster" },
            //new TechnicianSelector { Id = 5, TechnicianId = 447, DisplayText = "447 - Alex Aligner" }
        };

        protected override void OnParametersSet()
        {
            //if (Service?.Techs?.Count > 0)
            //{
            //    foreach (var tech in Service.Techs)
            //    {
            //        TechnicianSelector technician = Technicians.Find(technician => technician.TechnicianId == tech.TechnicianId);
            //        if (technician != null)
            //        {
            //            technician.Checked = true;
            //        }
            //    }
            //}
        }

        private void Save()
        {
            // Remove the techs that are no longer selected
            for (int i = Service.Techs.Count - 1; i >= 0; i--)
            {
                //TechnicianSelector technician = Technicians.Find(technician => 
                //    technician.Employment.Id == Service.Techs[i].Employment.Id && technician.Checked);

                //if (technician is null)
                //    Service.Techs.RemoveAt(i);
            }

            // Add the techs that weren't already selected
            foreach (var technician in Technicians)
            {
                //if (technician.Checked)
                //{
                //    RepairOrderTechToWrite tech = Service.Techs.FirstOrDefault(t => t.TechnicianId == technician.TechnicianId);
                //    if (tech == null)
                //    {
                //        Service.Techs.Add(new RepairOrderTechToWrite() { TechnicianId = technician.TechnicianId });
                //    }
                //}
            }

            OnDone.InvokeAsync();
        }

        private void Cancel()
        {
            OnDone.InvokeAsync();
        }

        public class TechnicianSelector
        {
            public RepairOrderServiceTechnicianToRead Technician { get; set; }
            public bool Checked { get; set; } = false;
        }
    }
}
