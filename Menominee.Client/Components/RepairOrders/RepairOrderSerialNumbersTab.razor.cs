using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {

        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int SerialNumbersMissingCount { get; set; }

        private bool EditDialogVisible { get; set; } = false;
        private bool CanEdit { get; set; } = false;

        public IList<RepairOrderSerialNumberToWrite> SerialNumberList { get; set; } = new List<RepairOrderSerialNumberToWrite>();
        public RepairOrderSerialNumberToWrite SerialNumberToEdit { get; set; }

        public void Save()
        {
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumberToEdit = args.Item as RepairOrderSerialNumberToWrite;
        }

        protected override void OnParametersSet()
        {
            BuildSerialNumberList();
        }

        private void BuildSerialNumberList()
        {
            foreach (var service in RepairOrder.Services)
            {
                foreach (var item in service.Items)
                {
                    foreach (var serialNumber in item.SerialNumbers)
                    {
                        SerialNumberList.Add(serialNumber);
                    }
                }
            }
        }

        private void UpdateMissingSerialNumberCount()
        {
            SerialNumbersMissingCount = RepairOrderHelper.MissingSerialNumberCount(RepairOrder.Services);
        }

        //private void UpdateSerialNumbers()
        //{
        //    // For each service... 
        //    foreach (var service in RepairOrder?.Services)
        //    {
        //        // ...update each RepairOrderItem collection of RepairOrderItemSerialNumbers
        //        foreach (var item in service?.Items)
        //        {
        //            // ...with the RepairOrderItem's edited SerialNumbers collection from SerialNumberList
        //            var updatedRows = SerialNumberList.FindAll(
        //                serialNumberItem =>
        //                serialNumberItem.RepairOrderItemId == item.Id
        //                && serialNumberItem.SerialNumber != null
        //                && !string.IsNullOrWhiteSpace(serialNumberItem.SerialNumber));

        //            foreach (var serialNumber in item.SerialNumbers)
        //            {
        //                // Find the matching row in SerialNumberList and update this serialNumber
        //                var serialNumberListItem = SerialNumberList.Find(
        //                serialNumberItem =>
        //                serialNumberItem.RepairOrderItemId == item.Id
        //                && serialNumberItem.SerialNumber != null
        //                && !string.IsNullOrWhiteSpace(serialNumberItem.SerialNumber));

        //                if (serialNumberListItem is not null)
        //                {
        //                    serialNumber.RepairOrderItemId = serialNumberListItem.RepairOrderItemId;
        //                    serialNumber.SerialNumber = serialNumberListItem.SerialNumber;
        //                }
        //            }

        //            item.SerialNumbers = UpdateSerialNumbers(updatedRows);
        //        }
        //    }

        //    UpdateMissingSerialNumberCount();
        //    Updated.InvokeAsync(SerialNumbersMissingCount);
        //}

        //private static List<RepairOrderSerialNumberToWrite> UpdateSerialNumbers(List<SerialNumberListItem> editedRows)
        //{
        //    var updatedRows = new List<RepairOrderSerialNumberToWrite>();

        //    foreach (var row in editedRows)
        //    {
        //        updatedRows.Add(new RepairOrderSerialNumberToWrite()
        //        {
        //            RepairOrderItemId = row.RepairOrderItemId,
        //            SerialNumber = row.SerialNumber
        //        });
        //    }

        //    return updatedRows;
        //}

        //private void AddExistingToSerialNumberList(RepairOrderItemToWrite item)
        //{
        //    foreach (var existingSerialNumber in item?.SerialNumbers)
        //    {
        //        SerialNumberListItem serialNumber = new SerialNumberListItem
        //        {
        //            ItemId = item.Id,
        //            RepairOrderItemId = existingSerialNumber.RepairOrderItemId,
        //            PartNumber = item.PartNumber,
        //            Description = item.Description,
        //            SerialNumber = existingSerialNumber.SerialNumber
        //        };

        //        SerialNumberList.Add(serialNumber);
        //    }
        //}

        //private void AddMissingToSerialNumberList(RepairOrderItemToWrite item)
        //{
        //    var matchingItemSerialNumbers = SerialNumberList.FindAll(
        //        serialNumber =>
        //        serialNumber.ItemId == item.Id);

        //    var missingItemSerialNumberRowsCount = item.QuantitySold - matchingItemSerialNumbers.Count;
        //    for (var i = 0; i < missingItemSerialNumberRowsCount; i++)
        //    {
        //        SerialNumberListItem serialNumber = new SerialNumberListItem
        //        {
        //            Id = 0,
        //            ItemId = item.Id,
        //            RepairOrderItemId = item.Id,
        //            PartNumber = item.PartNumber,
        //            Description = item.Description
        //        };

        //        SerialNumberList.Add(serialNumber);
        //    }
        //}

        //// TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        //private bool SerialNumberRequired(RepairOrderItemToWrite item)
        //{
        //    if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
        //    {
        //        // check if this part's product code requires serial numbers
        //        // if (ProductCodeRequiresSerialNumber())
        //        return true;
        //    }
        //    return false;
        //}
    }
}
