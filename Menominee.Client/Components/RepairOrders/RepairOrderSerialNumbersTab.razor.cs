using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {

        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        public List<SerialNumberListItem> SerialNumberList { get; set; } = new();

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int SerialNumbersMissingCount { get; set; }

        private bool EditDialogVisible { get; set; } = false;
        private bool CanEdit { get; set; } = false;
        public SerialNumberListItem SerialNumberToEdit { get; set; }

        public void Save()
        {
            UpdateSerialNumbers();
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumberToEdit = args.Item as SerialNumberListItem;
        }

        protected override void OnParametersSet()
        {
            BuildSerialNumberList();
        }

        private void BuildSerialNumberList()
        {
            // Search through each item on each service to find the ones needing serial numbers
            foreach (var service in RepairOrder?.Services)
            {
                foreach (var item in service?.Items)
                {
                    // check if serial numbers are required on current item
                    if (SerialNumberRequired(item))
                    {
                        // add existing serial number rows to the collection
                        AddExistingToSerialNumberList(item);

                        // add missing serial number rows for the current item
                        AddMissingToSerialNumberList(item);
                    }
                }
            }

            UpdateMissingSerialNumberCount();
        }

        private void UpdateMissingSerialNumberCount()
        {
            SerialNumbersMissingCount = SerialNumberList.FindAll(
                serialNumberListItem =>
                string.IsNullOrWhiteSpace(serialNumberListItem.SerialNumber)).Count;
        }

        private void UpdateSerialNumbers()
        {
            // For each service... 
            foreach (var service in RepairOrder?.Services)
            {
                // ...update each RepairOrderItem collection of RepairOrderItemSerialNumbers
                foreach (var item in service?.Items)
                {
                    // ...with the RepairOrderItem's edited SerialNumbers collection from SerialNumberList
                    var updatedRows = SerialNumberList.FindAll(
                        serialNumberItem =>
                        serialNumberItem.RepairOrderItemId == item.Id
                        && serialNumberItem.SerialNumber != null
                        && !string.IsNullOrWhiteSpace(serialNumberItem.SerialNumber));

                    foreach (var serialNumber in item.SerialNumbers)
                    {
                        // Find the matching row in SerialNumberList and update this serialNumber
                        var serialNumberListItem = SerialNumberList.Find(
                        serialNumberItem =>
                        serialNumberItem.RepairOrderItemId == item.Id
                        && serialNumberItem.SerialNumber != null
                        && !string.IsNullOrWhiteSpace(serialNumberItem.SerialNumber));

                        if (serialNumberListItem is not null)
                        {
                            serialNumber.RepairOrderItemId = serialNumberListItem.RepairOrderItemId;
                            serialNumber.SerialNumber = serialNumberListItem.SerialNumber;
                        }
                    }

                    item.SerialNumbers = UpdateSerialNumbers(updatedRows);
                }
            }

            UpdateMissingSerialNumberCount();
            Updated.InvokeAsync(SerialNumbersMissingCount);
        }

        private static List<RepairOrderSerialNumberToWrite> UpdateSerialNumbers(List<SerialNumberListItem> editedRows)
        {
            var updatedRows = new List<RepairOrderSerialNumberToWrite>();

            foreach (var row in editedRows)
            {
                updatedRows.Add(new RepairOrderSerialNumberToWrite()
                {
                    RepairOrderItemId = row.RepairOrderItemId,
                    SerialNumber = row.SerialNumber
                });
            }

            return updatedRows;
        }

        private void AddExistingToSerialNumberList(RepairOrderItemToWrite item)
        {
            foreach (var existingSerialNumber in item?.SerialNumbers)
            {
                SerialNumberListItem serialNumber = new SerialNumberListItem
                {
                    ItemId = item.Id,
                    RepairOrderItemId = existingSerialNumber.RepairOrderItemId,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    SerialNumber = existingSerialNumber.SerialNumber
                };

                SerialNumberList.Add(serialNumber);
            }
        }

        private void AddMissingToSerialNumberList(RepairOrderItemToWrite item)
        {
            var matchingItemSerialNumbers = SerialNumberList.FindAll(
                serialNumber =>
                serialNumber.ItemId == item.Id);

            var missingItemSerialNumberRowsCount = item.QuantitySold - matchingItemSerialNumbers.Count;
            for (var i = 0; i < missingItemSerialNumberRowsCount; i++)
            {
                SerialNumberListItem serialNumber = new SerialNumberListItem
                {
                    Id = 0,
                    ItemId = item.Id,
                    RepairOrderItemId = item.Id,
                    PartNumber = item.PartNumber,
                    Description = item.Description
                };

                SerialNumberList.Add(serialNumber);
            }
        }

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        private bool SerialNumberRequired(RepairOrderItemToWrite item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber())
                return true;
            }
            return false;
        }
    }
}
