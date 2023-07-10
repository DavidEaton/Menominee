using Menominee.Shared.Models.RepairOrders.Items;
using Menominee.Shared.Models.RepairOrders.LineItems;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderLineItemEditor : ComponentBase
    {
        [Parameter]
        public RepairOrderLineItemToWrite Line { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Item");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }
        
        private FormMode formMode;
        private string Title { get; set; }
        private List<SaleTypeEnumModel> SaleTypeEnumData { get; set; } = new();
        private List<LaborTypeEnumModel> LaborTypeEnumData { get; set; } = new();
        private List<DiscountTypeEnumModel> DiscountTypeEnumData { get; set; } = new();

        // replace the following when able
        private bool PlaceholderBuyout { get; set; } = false;
        private int PlaceholderQuantityOnHand { get; set; } = 0;
        private string PlaceholderReasonForReplacement { get; set; } = string.Empty;

        public List<ReasonForReplacement> ReasonsForReplacement = new()
        {
            new ReasonForReplacement { Code = "A", Type = ReasonForReplacementType.Required, Description = "Part no longer performs intended function" },
            new ReasonForReplacement { Code = "B", Type = ReasonForReplacementType.Required, Description = "Part does not meet a design specification" },
            new ReasonForReplacement { Code = "C", Type = ReasonForReplacementType.Required, Description = "Part is missing" },
            new ReasonForReplacement { Code = "D", Type = ReasonForReplacementType.Required, Description = "Necessary component of service" },
            new ReasonForReplacement { Code = "1", Type = ReasonForReplacementType.Suggested, Description = "Part is close to end of useful life" },
            new ReasonForReplacement { Code = "2", Type = ReasonForReplacementType.Suggested, Description = "Customer need, convenience or request" },
            new ReasonForReplacement { Code = "3", Type = ReasonForReplacementType.Suggested, Description = "Comply with recommended maintenance" },
            new ReasonForReplacement { Code = "4", Type = ReasonForReplacementType.Suggested, Description = "Technician's recommendation" }
        };

        protected override void OnInitialized()
        {
            foreach (SaleType item in Enum.GetValues(typeof(SaleType)))
            {
                SaleTypeEnumData.Add(new SaleTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(item), Value = item });
            }
            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                LaborTypeEnumData.Add(new LaborTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }
            foreach (ItemDiscountType item in Enum.GetValues(typeof(ItemDiscountType)))
            {
                DiscountTypeEnumData.Add(new DiscountTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }
        public class ReasonForReplacement
        {
            public string Code { get; set; }
            public ReasonForReplacementType Type { get; set; }
            public string Description { get; set; }
            public string DisplayText => $"{Code} - {Description}";
        }

    }
}
