using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
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

        public RepairOrderToWrite RepairOrder { get; set; }

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
                RepairOrder = ReadDtoToWriteDto(readDto);
                //RepairOrder = new RepairOrderToWrite()
                //{
                //    Id = readDto.Id,
                //    RepairOrderNumber = readDto.RepairOrderNumber,
                //    InvoiceNumber = readDto.InvoiceNumber,
                //    CustomerName = readDto.CustomerName,
                //    Vehicle = readDto.Vehicle,
                //    Total = readDto.Total
                //};
            }
        }

        public static RepairOrderToWrite ReadDtoToWriteDto(RepairOrderToRead repairOrderToRead)
        {
            var roToWrite = new RepairOrderToWrite
            {
                Id = repairOrderToRead.Id,
                RepairOrderNumber = repairOrderToRead.RepairOrderNumber,
                InvoiceNumber = repairOrderToRead.InvoiceNumber,
                CustomerName = repairOrderToRead.CustomerName,
                Vehicle = repairOrderToRead.Vehicle,
                PartsTotal = repairOrderToRead.PartsTotal,
                LaborTotal = repairOrderToRead.LaborTotal,
                DiscountTotal = repairOrderToRead.DiscountTotal,
                HazMatTotal = repairOrderToRead.HazMatTotal,
                TaxTotal = repairOrderToRead.TaxTotal,
                ShopSuppliesTotal = repairOrderToRead.ShopSuppliesTotal,
                Total = repairOrderToRead.Total
            };

            if (repairOrderToRead?.Services?.Count > 0)
            {
                foreach (var service in repairOrderToRead.Services)
                {
                    var serviceToWrite = new RepairOrderServiceToWrite
                    {
                        Id = service.Id,
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
                                        Id = sn.Id,
                                        RepairOrderItemId = sn.RepairOrderItemId,
                                        SerialNumber = sn.SerialNumber
                                    });
                                }
                            }

                            if (item.Taxes?.Count > 0)
                            {
                                foreach (var tax in item.Taxes)
                                {
                                    itemToWrite.Taxes.Add(new RepairOrderItemTaxToWrite()
                                    {
                                        Id = tax.Id,
                                        RepairOrderItemId = tax.RepairOrderItemId,
                                        SequenceNumber = tax.SequenceNumber,
                                        TaxId = tax.TaxId,
                                        PartTaxRate = tax.PartTaxRate,
                                        LaborTaxRate = tax.LaborTaxRate,
                                        PartTax = tax.PartTax,
                                        LaborTax = tax.LaborTax

                                    });
                                }
                            }

                            if (item.Warranties?.Count > 0)
                            {
                                foreach (var warranty in item.Warranties)
                                {
                                    itemToWrite.Warranties.Add(new RepairOrderWarrantyToWrite()
                                    {
                                        Id = warranty.Id,
                                        RepairOrderItemId = warranty.RepairOrderItemId,
                                        SequenceNumber = warranty.SequenceNumber,
                                        Quantity = warranty.Quantity,
                                        Type = warranty.Type,
                                        NewWarranty = warranty.NewWarranty,
                                        OriginalWarranty = warranty.OriginalWarranty,
                                        OriginalInvoiceId = warranty.OriginalInvoiceId
                                    });
                                }
                            }

                            serviceToWrite.Items.Add(itemToWrite);
                        }

                    }

                    if (service.Techs?.Count > 0)
                    {
                        foreach (var tech in service.Techs)
                        {
                            serviceToWrite.Techs.Add(new RepairOrderTechToWrite()
                            {
                                Id = tech.Id,
                                RepairOrderServiceId = tech.RepairOrderServiceId,
                                TechnicianId = tech.TechnicianId
                            });
                        }
                    }

                    if (service.Taxes?.Count > 0)
                    {
                        foreach (var tax in service.Taxes)
                        {
                            serviceToWrite.Taxes.Add(new RepairOrderServiceTaxToWrite()
                            {
                                Id = tax.Id,
                                RepairOrderServiceId = tax.RepairOrderServiceId,
                                SequenceNumber = tax.SequenceNumber,
                                TaxId = tax.TaxId,
                                PartTaxRate = tax.PartTaxRate,
                                LaborTaxRate = tax.LaborTaxRate,
                                PartTax = tax.PartTax,
                                LaborTax = tax.LaborTax
                            });
                        }
                    }

                    roToWrite.Services.Add(serviceToWrite);
                }
            }

            if (repairOrderToRead?.Payments.Count > 0)
            {
                foreach (var payment in repairOrderToRead.Payments)
                {
                    roToWrite.Payments.Add(new RepairOrderPaymentToWrite()
                    {
                        Id = payment.Id,
                        RepairOrderId = payment.RepairOrderId,
                        PaymentMethod = payment.PaymentMethod,
                        Amount = payment.Amount
                    });
                }
            }

            return roToWrite;
        }

        private async Task Save()
        {
            if (Valid())
            {
                //if (RepairOrder?.Services?.Count > 0)
                //{
                //    foreach (var service in RepairOrder.Services)
                //    {
                //        if (service.Id < 0)
                //            service.Id = 0;
                //        if (service.Items?.Count > 0)
                //        {
                //            foreach (var item in service.Items)
                //            {
                //                if (item.Id < 0)
                //                    item.Id = 0;
                //                item.RepairOrderServiceId = service.Id;
                //            }
                //        }
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
