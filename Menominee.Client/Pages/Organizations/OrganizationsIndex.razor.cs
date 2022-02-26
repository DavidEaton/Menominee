using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Components.Address;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Organizations
{
    public partial class OrganizationsIndex : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        [Inject]
        public ILogger<OrganizationsIndex> Logger { get; set; }

        public IReadOnlyList<OrganizationToReadInList> Organizations;

        public TelerikGrid<OrganizationToReadInList> Grid { get; set; }
        private long Id { get; set; }

        private FormMode OrganizationFormMode = FormMode.Unknown;

        private FormMode AddressFormMode = FormMode.Unknown;
        private OrganizationToWrite OrganizationToWrite { get; set; }

        private AddressEditor addressEditor;
        protected override async Task OnInitializedAsync()
        {
            Organizations = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private void AddAddress()
        {
            OrganizationToWrite.Address = new();
            AddressFormMode = FormMode.Add;
        }

        private void EditAddress()
        {
            AddressFormMode = FormMode.Edit;
        }
        private void CancelEditAddress()
        {
            if (AddressFormMode == FormMode.Edit)
            {
                addressEditor.Cancel();
                AddressFormMode = FormMode.Unknown;
            }

            if (OrganizationFormMode == FormMode.Add && OrganizationToWrite.Address is not null)
            {
                AddressFormMode = FormMode.Unknown;
                StateHasChanged();  //TODO: When Organization is new and Address is new, how to cancel?
                                    //Dialog does not close, console error: null reference exception
                OrganizationToWrite.Address = null;
            }
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as OrganizationToReadInList).Id;
            OrganizationFormMode = FormMode.Edit;
            Organizations = null;

            var readDto = await OrganizationDataService.GetOrganization(Id);
            OrganizationToWrite = new OrganizationToWrite
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                OrganizationToWrite.Address = new AddressToWrite
                {
                    AddressLine = readDto.Address.AddressLine,
                    City = readDto.Address.City,
                    State = readDto.Address.State,
                    PostalCode = readDto.Address.PostalCode
                };
            }

            if (readDto?.Emails.Count > 0)
            {
                foreach (var email in readDto.Emails)
                {
                    OrganizationToWrite.Emails.Add(new EmailToWrite
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    });
                }
            }

            if (readDto?.Phones.Count > 0)
            {
                foreach (var phone in readDto.Phones)
                {
                    OrganizationToWrite.Phones.Add(new PhoneToWrite
                    {
                        Number = phone.Number,
                        PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                        IsPrimary = phone.IsPrimary
                    });
                }
            }

        }

        private void Add()
        {
            OrganizationFormMode = FormMode.Add;
            Organizations = null;
            OrganizationToWrite = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(OrganizationToWrite.Name))
            {
                await OrganizationDataService.AddOrganization(OrganizationToWrite);
                await EndAddEditAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(OrganizationToWrite.Name))
            {
                await OrganizationDataService.UpdateOrganization(OrganizationToWrite, Id);
                await EndAddEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(OrganizationToWrite.Name))
            {
                if (OrganizationFormMode == FormMode.Add)
                    await HandleAddSubmit();

                if (OrganizationFormMode == FormMode.Edit)
                    await HandleEditSubmit();
            }
        }

        protected async Task EndAddEditAsync()
        {
            OrganizationFormMode = FormMode.Unknown;
            AddressFormMode = FormMode.Unknown;
            Organizations = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }
    }
}
