using Microsoft.AspNetCore.Components;
using Client.Services;
using System.Threading.Tasks;
using Migrations.Core.Entities;

namespace Client.Components
{
    public partial class AddPersonDialog
    {
        public Person Person { get; set; } = new Person();

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
            Person = new Person ();
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
