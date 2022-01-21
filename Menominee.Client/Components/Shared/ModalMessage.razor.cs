using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class ModalMessage : ComponentBase
    {
        //[CascadingParameter] BlazoredModalService BlazoredModal { get; set; }
        [CascadingParameter] IModalService ModalService { get; set; }

        [Parameter]
        public string Message { get; set; }

        //async Task CloseForm() => await ModalService.CloseAsync();
        //private void OkClick()
        //{
        //    //ModalService.Close(ModalResult.Ok(true));
        //}
    }
}
