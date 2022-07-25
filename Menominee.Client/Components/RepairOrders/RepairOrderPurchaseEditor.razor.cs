using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchaseEditor : ComponentBase
    {
        [Inject]
        public IVendorDataService vendorDataService { get; set; }

        [Parameter]
        public PurchaseListItem Purchase { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Purchase");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private IReadOnlyList<VendorToReadInList> Vendors = null;
        private FormMode formMode;
        private string Title { get; set; }
        private List<VendorX> VendorList = new List<VendorX>();

        protected override async Task OnParametersSetAsync()
        {
            Vendors = (await vendorDataService.GetAllVendorsAsync()).ToList();

            VendorList = new();
            foreach (var vendor in Vendors)
            {
                VendorList.Add(new VendorX
                {
                    Id = vendor.Id,
                    Code = vendor.VendorCode,
                    Name = vendor.Name
                });
            }
        }

        public class VendorX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Name;
                }
            }
        }
    }
}
