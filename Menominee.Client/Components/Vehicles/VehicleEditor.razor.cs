using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Menominee.Client.Components.Vehicles;

public partial class VehicleEditor
{
    [Parameter]
    public VehicleToRead Vehicle { get; set; }

    [Parameter]
    public FormMode FormMode { get; set; }

    [Parameter]
    public EventCallback<VehicleToWrite> OnSave { get; set; }

    [Parameter]
    public EventCallback OnDiscard { get; set; }

    private string Title => $"{FormMode} Vehicle";
    private EditContext EditContext { get; set; } = default!;
    private VehicleValidator VehicleValidator { get; set; } = new();
    private VehicleToWrite VehicleModel { get; set; } = new();
    private static IEnumerable<int> Years => Enumerable.Range(1896, DateTime.Now.Year - 1896 + 1).Reverse();
    List<StateProvinceEnumModel> StateProvinceEnumData { get; set; } = new List<StateProvinceEnumModel>();

    protected override void OnInitialized()
    {
        foreach (State item in Enum.GetValues(typeof(State)))
        {
            StateProvinceEnumData.Add(new StateProvinceEnumModel
            {
                DisplayText = item.ToString(),
                FullDisplayText = item.ToString() + " - " + EnumExtensions.GetDisplayName(item),
                Value = item
            });
        }
        StateProvinceEnumData = StateProvinceEnumData.OrderBy(state => state.DisplayText).ToList();
        
        if (FormMode.Equals(FormMode.Edit))
        {
            VehicleModel = VehicleHelper.ConvertReadToWriteDto(Vehicle);
        }

        EditContext = new EditContext(VehicleModel);
        base.OnInitialized();
    }

    private async Task HandleSubmit(EditContext editContext)
    {
        var isValid = editContext.Validate();

        if (!isValid) return;

        var vehicle = editContext.Model as VehicleToWrite;
        vehicle!.Id = VehicleModel.Id;

        await OnSave.InvokeAsync(vehicle);
    }

    private async Task HandleDiscard()
    {
        await OnDiscard.InvokeAsync();
    }

    public class StateEnumModel
    {
        public State Value { get; set; }
        public string DisplayText { get; set; }
        public string FullDisplayText { get; set; }
    }
}
