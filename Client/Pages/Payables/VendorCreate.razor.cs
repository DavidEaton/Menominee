using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using MenomineePlayWASM.Shared.Services.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using System;

namespace MenomineePlayWASM.Client.Pages.Payables
{
    //public partial class VendorCreate
    //{
    //    [Inject]
    //    private NavigationManager navigationManager { get; set; }

    //    //[Inject]
    //    //private IVendorRepository vendorRepository { get; set; }
    //    [Inject]
    //    public IVendorDataService VendorDataService { get; set; }

    //    [Parameter] 
    //    public long VendorId { get; set; }
    //    [Parameter]
    //    public VendorToAdd VendorToAdd { get; set; }

    //    [Parameter]
    //    public VendorToEdit VendorToEdit { get; set; }

    //    //private Vendor vendor = new Vendor();

    //    protected override async Task OnInitializedAsync()
    //    {
    //        if (VendorId == 0)
    //        {
    //            VendorToAdd = new();
    //        }
    //        else
    //        {
    //            var readDto = await VendorDataService.GetVendor(VendorId);
    //            VendorToEdit = new VendorToEdit
    //            {
    //                Name = readDto.Name,
    //                Note = readDto.Note
    //            };

    //            if (readDto.Address != null)
    //            {
    //                OrganizationToEdit.Address = new AddressToEdit
    //                {
    //                    AddressLine = readDto.Address.AddressLine,
    //                    City = readDto.Address.City,
    //                    State = readDto.Address.State,
    //                    PostalCode = readDto.Address.PostalCode
    //                };
    //            }

    //            if (readDto?.Emails.Count > 0)
    //            {
    //                foreach (var email in readDto.Emails)
    //                {
    //                    OrganizationToEdit.Emails.Add(new EmailToEdit
    //                    {
    //                        Address = email.Address,
    //                        IsPrimary = email.IsPrimary
    //                    });
    //                }
    //            }

    //            if (readDto?.Phones.Count > 0)
    //            {
    //                foreach (var phone in readDto.Phones)
    //                {
    //                    OrganizationToEdit.Phones.Add(new PhoneToEdit
    //                    {
    //                        Number = phone.Number,
    //                        PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
    //                        IsPrimary = phone.IsPrimary
    //                    });
    //                }
    //            }
    //        }
    //    }

    //    private void Create()
    //    {
    //        try
    //        {
    //            vendorRepository.CreateVendorAsync(vendor);
    //            navigationManager.NavigateTo("/payables/vendors/listing");
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }

    //    private void Discard()
    //    {
    //        navigationManager.NavigateTo("/payables/vendors/listing");
    //    }
    //}
}
