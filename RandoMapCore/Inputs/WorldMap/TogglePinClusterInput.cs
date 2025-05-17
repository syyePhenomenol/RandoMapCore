using RandoMapCore.Pins;
using RandoMapCore.UI;

namespace RandoMapCore.Input;

internal class TogglePinClusterInput : RmcWorldMapInput
{
    internal TogglePinClusterInput()
        : base("Toggle Overlapping Pins", () => InputHandler.Instance.inputActions.dreamNail)
    {
        Instance = this;
    }

    internal static TogglePinClusterInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnablePinSelection;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && RandoMapCoreMod.GS.PinSelectionOn;
    }

    public override void DoAction()
    {
        if (RmcPinManager.Selector?.SelectedObject is PinCluster pinCluster)
        {
            pinCluster.ToggleSelectedPin();
            PinSelectionPanel.Instance?.HideHint();
            PinSelectionPanel.Instance?.Update();
        }
    }
}
