using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderVehicle
    {
        //[Parameter]
        //public string YearMakeModel { get; set; }    // vehicle when repair order was saved

        //[Parameter]
        //public Vehicle Vehicle { get; set; }      // vehicle record as it exists now

        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        private void EditVehicle()
        {
        }

        private void AddVehicle()
        {
        }

        private void ViewNotes()
        {
        }
    }
}
