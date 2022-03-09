using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        public List<SerialNumberListItem> SerialNumberList { get; set; }

        [Parameter]
        public IReadOnlyList<RepairOrderServiceToRead> Services { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }
        private bool EditDialogVisible { get; set; } = false;
        private bool CanEdit { get; set; } = false;

        public IEnumerable<SerialNumberListItem> SelectedSerialNumbers { get; set; } = Enumerable.Empty<SerialNumberListItem>();
        public SerialNumberListItem SerialNumberToEdit { get; set; }

        protected override void OnInitialized()
        {

            if (Services?.Count > 0)
            {
                foreach (var service in Services)
                {
                    var serviceToWrite = new RepairOrderServiceToWrite
                    {
                        RepairOrderId = service.RepairOrderId,
                        SequenceNumber = service.SequenceNumber,
                        ServiceName = service.ServiceName,
                        SaleCode = service.SaleCode,
                        IsCounterSale = service.IsCounterSale,
                        IsDeclined = service.IsDeclined,
                        PartsTotal = service.PartsTotal,
                        LaborTotal = service.LaborTotal,
                        DiscountTotal = service.DiscountTotal,
                        TaxTotal = service.TaxTotal,
                        ShopSuppliesTotal = service.ShopSuppliesTotal,
                        Total = service.Total
                    };

                    if (service.Items?.Count > 0)
                    {
                        foreach (var item in service.Items)
                        {
                            var itemToWrite = new RepairOrderItemToWrite
                            {
                                Id = item.Id,
                                RepairOrderServiceId = item.RepairOrderServiceId,
                                SequenceNumber = item.SequenceNumber,
                                //Manufacturer = item.Manufacturer,
                                ManufacturerId = item.ManufacturerId,
                                PartNumber = item.PartNumber,
                                Description = item.Description,
                                //SaleCode = item.SaleCode,
                                SaleCodeId = item.SaleCodeId,
                                //ProductCode = item.ProductCode,
                                ProductCodeId = item.ProductCodeId,
                                SaleType = item.SaleType,
                                PartType = item.PartType,
                                IsDeclined = item.IsDeclined,
                                IsCounterSale = item.IsCounterSale,
                                QuantitySold = item.QuantitySold,
                                SellingPrice = item.SellingPrice,
                                LaborType = item.LaborType,
                                LaborEach = item.LaborEach,
                                DiscountType = item.DiscountType,
                                DiscountEach = item.DiscountEach,
                                Cost = item.Cost,
                                Core = item.Core,
                                Total = item.Total
                            };

                            if (item.SerialNumbers?.Count > 0)
                            {
                                foreach (var sn in item.SerialNumbers)
                                {
                                    itemToWrite.SerialNumbers.Add(new RepairOrderSerialNumberToWrite()
                                    {
                                        RepairOrderItemId = sn.RepairOrderItemId,
                                        SerialNumber = sn.SerialNumber
                                    });
                                }
                            }


                            serviceToWrite.Items.Add(itemToWrite);
                        }

                    }


                    Services.Add(serviceToWrite);
                }
            }




            SerialNumberList = new List<SerialNumberListItem> { SerialNumberToEdit };

        }

        public void Save()
        {
            EditDialogVisible = false;
            // Update SerialNumberList with model changes
            Updated.InvokeAsync();
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumberToEdit = args.Item as SerialNumberListItem;
        }

    }
}
