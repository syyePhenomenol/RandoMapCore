using RandoMapCore.Pins;
using RandoMapCore.UI;

namespace RandoMapCore.Input;

internal class LockGridPinInput : RmcWorldMapInput
{
    internal LockGridPinInput()
        : base("Lock Grid Pin", () => InputHandler.Instance.inputActions.dreamNail)
    {
        Instance = this;
    }

    internal static LockGridPinInput Instance { get; private set; }

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
        if (PinSelector.Instance?.SelectedObject is GridPin)
        {
            PinSelector.Instance.ToggleLockSelection();
            PinSelectionPanel.Instance.Update();
        }
    }
}
