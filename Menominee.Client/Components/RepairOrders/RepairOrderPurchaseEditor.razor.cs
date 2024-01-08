using CSharpFunctionalExtensions;
using Menominee.Client.Components.Settings;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Vendors;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchaseEditor : ComponentBase
    {
        [Inject]
        public IVendorDataService VendorDataService { get; set; }

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

        [Inject]
        ILogger<VendorInvoicePaymentMethodEditor> Logger { get; set; }

        private IReadOnlyList<VendorToRead> Vendors = null;
        private FormMode formMode;
        private string Title { get; set; }
        private List<VendorX> VendorList = new();

        protected override async Task OnParametersSetAsync()
        {
            await GetVendorsAsync();

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

        private async Task GetVendorsAsync()
        {
            if (VendorDataService is not null)
            {
                await VendorDataService.GetAllAsync()
                .Match(
                    success => Vendors = success
                        .Where(vendor =>
                               vendor.IsActive == true
                            && vendor.VendorRole == VendorRole.PaymentReconciler)
                        .OrderBy(vendor => vendor.VendorCode)
                        .ToList(),

                    failure => Logger.LogError(failure));
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
