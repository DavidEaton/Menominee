using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderCustomer : ComponentBase
    {
        public string CustName { get; set; }

        private bool CanEditCust { get; set; } = true;
        private bool CanAddCust { get; set; } = true;

        private void OnAddCust()
        {
            CustName = "Add Customer";
        }

        private void OnEditCust()
        {
            CustName = "Edit Customer";
        }
    }
}
