using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class AddPersonDialog
    {
        public PersonDto Person { get; set; } = new PersonDto();
        public EntityType EntityType { get; set; }

        [Inject]
        public IPersonDataService PersonDataService { get; set; }
        public bool ShowDialog { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public void Show()
        {
            ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        private void ResetDialog()
        {
            Person = new PersonDto();
        }

        protected async Task HandleValidSubmit()
        {
            await PersonDataService.AddPerson(Person);
            ShowDialog = false;

            await CloseEventCallback.InvokeAsync(true);
            StateHasChanged();
        }
    }
}
