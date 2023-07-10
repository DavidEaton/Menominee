using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.Warranties;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderWarrantyEditor : ComponentBase
    {
        [Parameter]
        public WarrantyListItem Warranty { get; set; }

        [CascadingParameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Warranty");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        protected override void OnInitialized()
        {
            foreach (WarrantyType item in Enum.GetValues(typeof(WarrantyType)))
                WarrantyTypeEnumData.Add(new WarrantyTypeEnumModel
                {
                    DisplayText = item.ToString(),
                    Value = item
                });

            Mode = FormMode.Edit;
        }

        private FormMode formMode;
        private string Title { get; set; }
        List<WarrantyTypeEnumModel> WarrantyTypeEnumData { get; set; } = new List<WarrantyTypeEnumModel>();
    }
    public class WarrantyTypeEnumModel
    {
        public WarrantyType Value { get; set; }
        public string DisplayText { get; set; }
    }
}
