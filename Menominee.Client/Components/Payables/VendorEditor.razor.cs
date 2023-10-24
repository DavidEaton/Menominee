using CSharpFunctionalExtensions;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Client.Shared;
using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Menominee.Client.Components.Payables;

public partial class VendorEditor : ComponentBase
{
    [Inject]
    public IVendorInvoicePaymentMethodDataService? PaymentMethodDataService { get; set; }

    [Parameter]
    public VendorToWrite? Vendor { get; set; }

    [Parameter]
    public EventCallback OnValidSubmit { get; set; }

    [Parameter]
    public EventCallback OnDiscard { get; set; }

    [Parameter]
    public FormMode FormMode { get; set; }

    [Inject]
    ILogger<VendorEditor> Logger { get; set; }

    private string Title { get; set; } = string.Empty;
    private IList<VendorTypeEnumModel> VendorTypeEnumData { get; set; } = new List<VendorTypeEnumModel>();
    private ObservableCollection<PaymentTypeModel> PaymentMethodList { get; set; } = new ObservableCollection<PaymentTypeModel>();
    protected long PaymentMethodId { get; set; } = 0;
    protected bool AutoComplete { get; set; } = false;
    private bool parametersSet = false;
    private IReadOnlyList<VendorInvoicePaymentMethodToReadInList> paymentMethodList = new List<VendorInvoicePaymentMethodToReadInList>();
    protected override async Task OnInitializedAsync()
    {
        foreach (VendorRole type in Enum.GetValues(typeof(VendorRole)))
        {
            VendorTypeEnumData.Add(new VendorTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(type), Value = type });
        }

        await GetPaymentMethodList();

        PaymentMethodList.Add(new PaymentTypeModel { Value = 0, DisplayText = "None" });
        foreach (var method in paymentMethodList)
        {
            PaymentMethodList.Add(new PaymentTypeModel { Value = method.Id, DisplayText = method.Name });
        }
    }

    private async Task GetPaymentMethodList()
    {
        if (PaymentMethodDataService is not null)
        {
            await PaymentMethodDataService.GetAllAsync()
            .Match(
                success => paymentMethodList = success,
                failure => Logger.LogError(failure));
        }
    }

    protected override void OnParametersSet()
    {
        if (parametersSet)
        {
            return;
        }
        parametersSet = true;

        Title = FormTitle.BuildTitle(FormMode, "Vendor");

        if (Vendor is not null && Vendor.Address is null)
        {
            Vendor.Address = new();
        }
        PaymentMethodId = Vendor?.DefaultPaymentMethod?.PaymentMethod.Id ?? 0;
        AutoComplete = Vendor?.DefaultPaymentMethod?.AutoCompleteDocuments ?? false;
    }

    private void OnPaymentMethodChange()
    {
        if (PaymentMethodId == 0)
        {
            AutoComplete = false;
        }
    }

    protected async Task OnSaveAsync()
    {
        if (Vendor is null)
        {
            return;
        }
            
        if (!AddressIsValid(Vendor.Address))
        {
            Vendor.Address = null;
        }

        if (PaymentMethodId == 0)
        {
            Vendor.DefaultPaymentMethod = null;
        }
        else if (PaymentMethodDataService is not null)
        {
            var result = await PaymentMethodDataService.GetAsync(PaymentMethodId);

            if (result.IsFailure)
            {
                Logger.LogError(result.Error);
                return;
            }

            Vendor.DefaultPaymentMethod = new()
            {
                PaymentMethod = result.Value,
                AutoCompleteDocuments = AutoComplete
            };
        }

        await OnValidSubmit.InvokeAsync(this);
    }

    private static bool AddressIsValid(AddressToWrite address)
    {
        if (address is null)
        {
            return false;
        }

        var validator = new AddressValidator();

        return validator.Validate(address).IsValid;
    }

    protected class PaymentTypeModel
    {
        public long Value { get; set; }
        public string DisplayText { get; set; } = string.Empty;
    }
}
