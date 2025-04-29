namespace RandoMapCore.Input;

internal class PinPanelInput : MapUIKeyInput
{
    internal PinPanelInput()
        : base("Toggle Pin Panel", UnityEngine.KeyCode.P)
    {
        Instance = this;
    }

    internal static PinPanelInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnablePinSelection;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.TogglePinSelection();
        base.DoAction();
    }
}
