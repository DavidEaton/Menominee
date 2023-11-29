using CSharpFunctionalExtensions;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Payables.Invoices;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables.Pages;

public partial class VendorInvoiceEditPage : ComponentBase
{
    private const string GeneralFailureMessage = "An error occurred while saving the invoice";

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private IVendorInvoiceDataService? VendorInvoiceDataService { get; set; }

    [Inject]
    private ILogger<VendorInvoiceEditPage>? Logger { get; set; }

    [Parameter]
    public long Id { get; set; }

    private VendorInvoiceToWrite? Invoice { get; set; }
    private FormMode FormMode { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
        {
            Invoice = new()
            {
                Date = DateTime.Today,
                Status = VendorInvoiceStatus.Open
            };

            FormMode = FormMode.Add;
        }
        else if (VendorInvoiceDataService is not null)
        {
            var result = await VendorInvoiceDataService.GetAsync(Id);
            if (result.IsSuccess)
            {
                Invoice = VendorInvoiceHelper.ConvertReadToWriteDto(result.Value);
                FormMode = (Invoice.Status == VendorInvoiceStatus.Open) ? FormMode.Edit : FormMode.View;
            }
            else
            {
                // TODO: What to do?
                // Alert user that the Invoice could not be found
                FormMode = FormMode.View;
            }
        }
    }
    private async Task<Result<string>> Save()
    {
        if (Valid())
        {
            const string addInvoiceSuccessMessage = "Invoice added successfully";
            const string addInvoiceFailureMessage = "Failed to add new invoice";
            const string updateInvoiceSuccessMessage = "Invoice updated successfully";
            const string updateInvoiceFailureMessage = "Failed to update invoice";

            try
            {
                if (VendorInvoiceDataService is not null && Invoice is not null)
                {
                    Result<string> operationResult;

                    if (Id == 0)
                    {
                        var addInvoiceResult = await VendorInvoiceDataService.AddAsync(Invoice);
                        operationResult = addInvoiceResult.IsSuccess
                            ? Result.Success(addInvoiceSuccessMessage)
                            : Result.Failure<string>(addInvoiceFailureMessage);

                        if (addInvoiceResult.IsSuccess)
                        {
                            Id = Invoice.Id;
                        }
                    }
                    else
                    {
                        var updateInvoiceResult = await VendorInvoiceDataService.UpdateAsync(Invoice);
                        operationResult = updateInvoiceResult.IsSuccess
                            ? Result.Success(updateInvoiceSuccessMessage)
                            : Result.Failure<string>(updateInvoiceFailureMessage);
                    }

                    if (operationResult.IsSuccess)
                    {
                        // Un-comment this line if you want to show a toast for success
                        // toastService.ShowSuccess(operationResult.Value, "Success");
                    }

                    return operationResult;
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, GeneralFailureMessage);
                return Result.Failure<string>(GeneralFailureMessage);
            }
            finally
            {
                EndEdit();
            }
        }

        return Result.Failure<string>(GeneralFailureMessage);
    }

    private async Task SaveAndExit()
    {
        var result = await Save();

        if (result.IsSuccess)
        {
            EndEdit();
        }
        else
        {
            Logger?.LogError(result.Error, GeneralFailureMessage);
        }
    }

    private bool Valid()
    {
        return new VendorInvoiceRequestValidator()
            .Validate(Invoice)
            .IsValid;
    }

    private void Discard()
    {
        EndEdit();
    }

    protected void EndEdit()
    {
        // TODO: Need to preserve the listing's filter settings upon return
        NavigationManager?.NavigateTo($"payables/invoices/listing/{Id}");
    }
}
