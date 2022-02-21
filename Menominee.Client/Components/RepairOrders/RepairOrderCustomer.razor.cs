using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderCustomer : ComponentBase
    {
        //[Parameter]
        //public string CustomerName { get; set; }    // customer name as it was when repair order was saved

        //[Parameter]
        //public Customer Customer { get; set; }      // customer record as it exists now

        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        //private string CustName { get; set; }

        //private bool CanEditCust { get; set; } = true;
        //private bool CanAddCust { get; set; } = true;

        //private void OnAddCust()
        //{
        //    CustName = "Add Customer";
        //}

        //private void OnEditCust()
        //{
        //    CustName = "Edit Customer";
        //}
    }
}
