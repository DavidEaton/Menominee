using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders.Pages
{
    public partial class RepairOrderEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IRepairOrderDataService DataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        public RepairOrderToRead RepairOrder { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                RepairOrder = new()
                {
                    DateCreated = DateTime.Today
                };
            }
            else
            {
                RepairOrder = await DataService.GetRepairOrder(Id);
            }
        }

        private async Task Save()
        {
            if (Valid())
            {
                if (Id == 0)
                {
                    //await DataService.AddRepairOrder(RepairOrderToEdit);
                }
                else
                {
                    //await DataService.UpdateRepairOrder(RepairOrderToEdit, Id);
                }

                EndEdit();
            }
        }

        private bool Valid()
        {
            //if (Invoice.VendorId > 0 && Invoice.Date.HasValue)
            //    return true;

            //return false;
            return true;
        }

        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            NavigationManager.NavigateTo($"repairorders/worklog/{Id}");
        }
    }
}
