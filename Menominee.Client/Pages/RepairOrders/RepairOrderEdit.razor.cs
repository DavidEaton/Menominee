using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Menominee.Client.Services.RepairOrders;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Pages.RepairOrders
{
    public partial class RepairOrderEdit : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private IRepairOrderDataService dataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        private RepairOrderToWrite RepairOrder { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                RepairOrder = new();
                //Invoice.Date = DateTime.Today;
            }
            else
            {
                var readDto = await dataService.GetRepairOrder(Id);
                RepairOrder = new RepairOrderToWrite()
                {
                    Id = readDto.Id,
                    RepairOrderNumber = readDto.RepairOrderNumber,
                    InvoiceNumber = readDto.InvoiceNumber,
                    CustomerName = readDto.CustomerName,
                    Vehicle = readDto.Vehicle,
                    Total = readDto.Total
                };
            }
        }

        private async Task Save()
        {
            if (Valid())
            {
                //if (Invoice.LineItems != null)
                //{
                //    foreach (var item in Invoice.LineItems)
                //    {
                //        if (item.Id < 0)
                //            item.Id = 0;
                //    }
                //}

                //if (Invoice.Taxes != null)
                //{
                //    foreach (var tax in Invoice.Taxes)
                //    {
                //        if (tax.Id < 0)
                //            tax.Id = 0;
                //    }
                //}

                //if (Invoice.Payments != null)
                //{
                //    foreach (var payment in Invoice.Payments)
                //    {
                //        if (payment.Id < 0)
                //            payment.Id = 0;
                //    }
                //}

                if (Id == 0)
                {
                    var ro = await dataService.AddRepairOrder(RepairOrder);
                    Id = ro.Id;
                }
                else
                {
                    await dataService.UpdateRepairOrder(RepairOrder, Id);
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
            navigationManager.NavigateTo($"repairorders/worklog/{Id}");
        }
    }
}
