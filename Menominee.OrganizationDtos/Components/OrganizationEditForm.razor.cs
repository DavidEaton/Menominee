using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Components
{
    public partial class OrganizationEditForm : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        public OrganizationReadDto OrganizationRead { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public long Id { get; set; }

        [Parameter]
        public EventCallback<bool> OnEditCompleted { get; set; }
        protected OrganizationUpdateDto OrganizationToUpdate { get; set; }
        protected OrganizationAddDto OrganizationToAdd { get; set; }

        protected override async Task OnInitializedAsync()
        {

            switch (FormMode)
            {
                case FormMode.Add:
                    OrganizationToAdd = new();
                    break;
                case FormMode.Edit:
                    OrganizationToUpdate = new();

                    if (Id != 0)
                        OrganizationRead = await OrganizationDataService.GetOrganization(Id);

                    if (OrganizationRead != null)
                    {
                        OrganizationToUpdate.Name = OrganizationRead.Name;
                        OrganizationToUpdate.Note = OrganizationRead.Note;
                    }

                    break;
                default:
                    break;
            }
        }

        protected async Task HandleUpdateSubmit()
        {
            if (OrganizationToUpdate.Name != null && !string.IsNullOrWhiteSpace(OrganizationToUpdate.Name))
                await OrganizationDataService.UpdateOrganization(OrganizationToUpdate, Id);

            Close(true);
        }

        public void Close(bool saved)
        {
            FormMode = FormMode.Hidden;
            OnEditCompleted.InvokeAsync(saved);
        }
    }

}
